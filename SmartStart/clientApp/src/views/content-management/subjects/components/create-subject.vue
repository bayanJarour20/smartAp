<template>
<ValidationObserver ref="observer">
    <b-form>
        <EKDialog
           :title="title"
            @ok="onSubmit"
            @open="$store.commit('Reset_Subject_Dto')"
            endClose
            :placeholder="!isEdit ? 'ابحث عن مادة محددة' : ''"
            :btnText="!isEdit ? 'مادة جديد' : ''"
            @search="search"
            ref="subjectDialog"
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
                    v-model="subjectDto.doctors"
                    label="مدرسو المادة"
                    placeholder="اختر مدرسو المادة"
                    :options="doctors"
                    name="tagIds"
                    multiple
                    :clearable="true"
                />
                <b-form-group label="نوع المادة">
                    <div class="d-flex justify-content-between">
                        <b-form-radio v-model="subjectDto.type" value="0">نظري</b-form-radio>
                        <b-form-radio v-model="subjectDto.type" value="1">عملي</b-form-radio>
                    </div>
                </b-form-group>
                <b-form-group label="سعر المادة">
                    <div class="d-flex justify-content-between">
                        <b-form-radio v-model="subjectDto.isFree" value="true">مجانية</b-form-radio>
                        <b-form-radio v-model="subjectDto.isFree" value="false">ليست مجانية</b-form-radio>
                    </div>
                </b-form-group>
                <EKInputImage
                    label="صورة المادة"
                    title="upload image"
                    v-model="subjectDto.imagePath"
                    
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
       props: {
        title: {
            type: String,
            default: () => "إضافة مادة"
        },
        isEdit: Boolean
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
                    subjectFormData.append('type', this.subjectDto.type)
                    subjectFormData.append('description', this.subjectDto.description)
                    subjectFormData.append('isFree', this.subjectDto.isFree)
                   
                        
                        subjectFormData.append('imagePath', this.subjectDto.imagePath)
                    
                     this.subjectDto.doctors.forEach((doctorId, index) => {
                        subjectFormData.append('doctors[' + index + ']', doctorId);
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
        },
         openDialog() {
            this.$refs.subjectDialog.open();
        },
    }
};
</script>
