<template>
<ValidationObserver ref="observer">
    <b-form>
        <EKDialog
            title="إضافة مادة"
            placeholder="ابحث عن مادة محددة"
            btnText="مادة جديد"
            @ok="onSubmit"
            @open="$store.commit('Reset_Subject_Dto')"
            endClose
            @search="search"
        >
            <template #body>
                <div class="d-flex justify-content-center">
                <span>مجانية</span>
                <b-form-checkbox switch v-model="subjectDto.isFree"></b-form-checkbox>
                <span>مدفوعة</span>
                </div>          
                 <EKInputText
                    label="اسم المادة"
                    placeholder="ادخل اسم المادة"
                    :rules="[
                        {
                            type: 'required',
                            message: 'اسم المادة إجبارية'
                        }
                    ]"
                  v-model="subjectDto.name"
                    name="name"
                    
                />
                <EKInputSelect
                    v-model="subjectDto.facultyId"
                    label="الكلية"
                    placeholder="اختر الكلية"
                    :rules="[
                        {
                            type: 'required',
                            message: 'الكلية إجبارية'
                        }
                    ]"
                    :options="faculties"
                    name="select"
                    :clearable="true"
                />
                <EKInputText
                    v-model="subjectDto.year"
                    :rules="[{ type: 'required', message: 'سنة المادة إجباري' }, { type: 'min_value:0', message: 'الحقل يجب ان يحوي قيمة موجبة' }]"
                    label="سنة المادة"
                    placeholder="ادخل سنة المادة"
                    type="number"
                    name="year"
                />
                
                <EKInputSelect
                    v-model="subjectDto.semesterId"
                    label="الفصل"
                    placeholder="اختر الفصل"
                    :rules="[{ type: 'required', message: 'الفصل إجباري' }]"
                    :options="semester"
                    name="semesterId"
                    :clearable="true"
                />
                <EKInputSelect
                    v-model="subjectDto.tagIds"
                    label="مدرسو المادة"
                    placeholder="اختر مدرسو المادة"
                      :options="doctors"
                    name="tagIds"
                    multiple
                    :clearable="true"
                />
                <EKInputSelect
                    label="القسم"
                    placeholder="اختر القسم"
                    :rules="[{ type: 'required', message: 'القسم إجباري' }]"
                    :options="tagsList"
                    v-model="tags"
                    multiple
                    name="tags"
                    :clearable="true"
                />
                <b-form-group label="نوع المادة">
                    <div class="d-flex justify-content-between">
                        <b-form-radio v-model="subjectDto.type" value="0">نظري</b-form-radio>
                        <b-form-radio v-model="subjectDto.type" value="1">عملي</b-form-radio>
                    </div>
                </b-form-group>
                <b-button variant="primary" class="w-100 my-1"><unicon                
                    name="plus"
                    width="18"
                    height="18"
                    fill="#fff"
                /> </b-button>
                <div class="div-subject">
                    كلية العلوم -قسم الأحياء-الفصل الأول
                    <unicon
                 
                    name="times"
                    width="18"
                    height="18"
                    fill="#7367f0"
                /> 
                </div>
                <EKInputImage
                    label="صورة المادة"
                    title="upload image"
                    :value="subjectDto.imagePath"
                    @input="subjectDto.file = $event"
                ></EKInputImage>
                <EKInputTextarea
                    v-model="subjectDto.description"
                    label=" شرح الصورة"
                    placeholder="ادخل شرح الصورة"
                    name="address"
                />
            </template>
        </EKDialog>
    </b-form>
</ValidationObserver>
</template>
<script>

import { ValidationObserver } from "vee-validate";
import EKDialog from "@Ekcore/components/EK-dialog";
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";
import EKInputImage from "@Ekcore/components/EK-forms/EK-input-image";
import { mapActions, mapState, mapGetters } from 'vuex';
import EKInputTextarea from "@Ekcore/components/EK-forms/EK-input-textarea";
import EKInputText from "@Ekcore/components/EK-forms/EK-input-text";

export default {
    components: {
        ValidationObserver,
        EKDialog,
        EKInputImage,
        EKInputSelect,
        EKInputTextarea,
        EKInputText
        
    },
    data: () => ({
        tags: []
    }),
    computed: {
        ...mapGetters(["tagsList", "semester", "doctors"]),
        ...mapState({
            subjectDto: state => state.subjects.subjectDto,
            faculties: state => state.faculties.faculties
        })
    },
    created() {
        this.getFacultiesDetails();
    },
    methods: {
        ...mapActions(["uploadSubject", "getFacultiesDetails"]),
        onSubmit() {
            this.$refs.observer.validate().then(success => {
                if (success) {
                    var subjectFormData = new FormData();
                    subjectFormData.append('name', this.subjectDto.name)
                    subjectFormData.append('file', this.subjectDto.file)
                    subjectFormData.append('type', this.subjectDto.type)
                    subjectFormData.append('facultyId', this.subjectDto.facultyId)
                    subjectFormData.append('semesterId', this.subjectDto.semesterId)
                    subjectFormData.append('description', this.subjectDto.description)
                    subjectFormData.append('isFree', this.subjectDto.isFree)
                    if(this.subjectDto.imagePath) {
                        subjectFormData.append('imagePath', this.subjectDto.imagePath)
                    }
                    this.subjectDto.tagIds.forEach((doctorId, index) => {
                        subjectFormData.append('tagIds[' + index + ']', doctorId);
                    })
                    
                    this.tags.forEach((doctorId, index) => {
                        subjectFormData.append('tagIds[' + (this.subjectDto.tagIds.length + index) + ']', doctorId);
                    })
                    
                    this.uploadSubject({
                        id: this.subjectDto.id,
                        formData: subjectFormData
                    })
                }
            });
        },
        search(query) {
            this.$store.commit('Set_Search_Dto', {
                keys: ['name'],
                query
            })
        }
    }
};
</script>
<style>
.div-subject{
    border: 1px solid #7367f0 ;
    border-radius: 25px;
    padding: 11px;
    text-align: center;
    margin: 17px 0px;
    color:#7367f0;
}
</style>