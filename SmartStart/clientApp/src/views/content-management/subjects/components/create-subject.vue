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
                <EKInputText
                    v-model="subjectDto.name"
                    :rules="[{ type: 'required', message: 'اسم المادة إجباري' }]"
                    label="اسم المادة"
                    placeholder="ادخل اسم المادة"
                    name="name"
                />
                <EKInputTextarea
                    v-model="subjectDto.description"
                    label="شرح المادة"
                    placeholder="ادخل شرح المادة"
                    name="description"
                />
                <EKInputSelect
                    v-model="subjectDto.facultyId"
                    label="تابعة لكلية"
                    placeholder="اختر تابعة لكلية"
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
                    label="تصنيف المادة"
                    placeholder="اختر تصنيف المادة"
                    :rules="[{ type: 'required', message: 'تصنيف المادة إجباري' }]"
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
                <EKInputImage
                    label="صورة المادة"
                    title="upload image"
                    :value="subjectDto.imagePath"
                    @input="subjectDto.file = $event"
                ></EKInputImage>
            </template>
        </EKDialog>
    </b-form>
</ValidationObserver>
</template>
<script>

import { ValidationObserver } from "vee-validate";
import EKDialog from "@Ekcore/components/EK-dialog";
import EKInputText from "@Ekcore/components/EK-forms/EK-input-text";
import EKInputTextarea from "@Ekcore/components/EK-forms/EK-input-textarea";
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";
import EKInputImage from "@Ekcore/components/EK-forms/EK-input-image";
import { mapActions, mapState, mapGetters } from 'vuex';

export default {
    components: {
        ValidationObserver,
        EKDialog,
        EKInputText,
        EKInputImage,
        EKInputSelect,
        EKInputTextarea
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
                    subjectFormData.append('year', this.subjectDto.year)
                    subjectFormData.append('file', this.subjectDto.file)
                    subjectFormData.append('type', this.subjectDto.type)
                    subjectFormData.append('facultyId', this.subjectDto.facultyId)
                    subjectFormData.append('semesterId', this.subjectDto.semesterId)
                    subjectFormData.append('description', this.subjectDto.description)
                    
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
