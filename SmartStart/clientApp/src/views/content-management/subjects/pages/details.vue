<template>
    <ValidationObserver ref="observer">
        <b-form @submit.prevent="onSubmit">
            <b-card no-body class="mb-2">
                <b-card-header class="align-items-center">
                    <h4 class="mr-auto mb-0">تفاصيل المادة</h4>
                </b-card-header>
                <b-card-body>
                    <b-card-text>
                        <b-row>
                            <b-col cols="12" md="6">
                                <EKInputText
                                    v-model="subjectDto.name"
                                    :rules="[
                                        {
                                            type: 'required',
                                            message: 'اسم المادة إجباري'
                                        }
                                    ]"
                                    label="اسم المادة"
                                    placeholder="ادخل اسم المادة"
                                    name="name"
                                />
                                <EKInputSelect
                                    v-model="subjectDto.facultyId"
                                    label="تابعة لكلية"
                                    placeholder="اختر تابعة لكلية"
                                    :rules="[
                                        {
                                            type: 'required',
                                            message:
                                                ' أدخل الكلية التي تكون المادة التابعة لها'
                                        }
                                    ]"
                                    :options="faculties"
                                    name="facultyId"
                                />
                                <EKInputSelect
                                    v-model="subjectDto.semesterId"
                                    label="الفصل"
                                    placeholder="اختر الفصل"
                                    :rules="[
                                        {
                                            type: 'required',
                                            message: 'الفصل إجباري'
                                        }
                                    ]"
                                    :options="semester"
                                    name="semesterId"
                                    :clearable="true"
                                />
                                    <EKInputSelect
                                    v-model="subjectDto.sectionId"
                                    label="القسم"
                                    placeholder="قسم عام"
                                   
                                    :options="sections"
                                    name="sectionId"
                                    :clearable="true"
                                />
                                <EKInputSelect
                                    v-model="subjectDto.doctorsId"
                                    label="مدرسو المادة"
                                    placeholder="اختر مدرسو المادة"
                                  
                                    :options="doctors"
                                    multiple
                                    name="doctorsId"
                                    :clearable="true"
                                />
                                <EKInputSelect
                                    label="تصنيف المادة"
                                    placeholder="اختر تصنيف المادة"
                                     :options="tagsList"
                                    v-model="subjectDto.tagIds"
                                    multiple
                                    name="tags"
                                    :clearable="true"
                                />
                                <EKInputText
                                    v-model="subjectDto.year"
                                    :rules="[{ type: 'required', message: 'سنة المادة إجبارية' }, { type: 'min_value:0', message: 'الحقل يجب ان يحوي قيمة موجبة' }]"
                                    label="سنة المادة"
                                    placeholder="ادخل سنة المادة"
                                    type="number"
                                    name="year"
                                />
                                 <EKInputTextarea
                                    v-model="subjectDto.description"
                                    label="شرح المادة"
                                    placeholder="ادخل شرح المادة"
                                    name="description"
                                />
                                
                            </b-col>
                            <b-col cols="12" md="6">
                           
                                <EKInputImage
                                    label="صورة المادة"
                                    required
                                    title="upload image"
                                    :value="
                                        $store.getters['app/domainHost'] +
                                            '/' +
                                            subjectDto.imagePath
                                    "
                                    @input="subjectDto.file = $event"
                                ></EKInputImage>
                                <b-form-group label="تصنيف المادة">
                                    <div class="d-flex justify-content-between">
                                        <b-form-radio
                                            v-model="subjectDto.type"
                                            value="0"
                                            >نظري</b-form-radio
                                        >
                                        <b-form-radio
                                            v-model="subjectDto.type"
                                            value="1"
                                            >عملي</b-form-radio
                                        >
                                    </div>
                                </b-form-group>
                                    <EKInputText
                                    v-model="subjectDto.examCount"
                                    type="number"
                                    label="عدد الدورات"
                                    readonly
                                    name="examCount"
                                />
                                <EKInputText
                                    v-model="subjectDto.bankCount"
                                    type="number"
                                    label="عدد بنوك الاسئلة"
                                    readonly
                                    name="bankCount"
                                />
                                <EKInputText
                                    v-model="subjectDto.interviewCount"
                                    type="number"
                                    label="عدد الدورات الكتابية"
                                    readonly
                                    name="interviewCount"
                                />
                                <EKInputText
                                    v-model="subjectDto.microscopeCount"
                                    type="number"
                                    label="عدد المجاهر"
                                    readonly
                                    name="microscopeCount"
                                />
                            </b-col>
                        </b-row>
                    </b-card-text>
                </b-card-body>
                <b-card-footer>
                    <b-row>
                        <b-col>
                            <div class="d-flex">
                                <b-button
                                    class="mr-1"
                                    type="submit"
                                    variant="primary"
                                    style="max-width:100px"
                                    >تعديل</b-button
                                >
                                <b-button
                                    variant="outline-primary"
                                    style="max-width:100px"
                                    to="../subjects"
                                    >تراجع</b-button
                                >
                            </div>
                        </b-col>
                        <b-col
                            style="
                                    display: flex;
                                    justify-content: flex-end;"
                        >
                            <b-button
                                style="max-width:100px"
                                variant="outline-primary"
                                     @click="deleteSubject(subjectDto.id)"
                                >حذف</b-button
                            >
                        </b-col>
                    </b-row>
                </b-card-footer>
            </b-card>
        </b-form>
    </ValidationObserver>
</template>
<script>
import { ValidationObserver } from "vee-validate";
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";
import EKInputImage from "@Ekcore/components/EK-forms/EK-input-image";
import EKInputText from "@Ekcore/components/EK-forms/EK-input-text";
import EKInputTextarea from "@Ekcore/components/EK-forms/EK-input-textarea";

import { mapActions, mapState, mapGetters } from "vuex";

export default {
    props: {
        id: String
    },
    components: {
        ValidationObserver,
        EKInputSelect,
        EKInputText,
        EKInputImage,
        EKInputTextarea
    },
    computed: {
        ...mapGetters(["semester", "doctors", "tagsList","sections"]),
        ...mapState({
            subjectDto: state => state.subjects.subjectDto,
            faculties: state => state.faculties.faculties
        })
    },
    created() {
        this.fetchTotalTag()
        this.subjectDetails(this.id);
        this.getFacultiesDetails();
    },
    methods: {
        ...mapActions([
            "subjectDetails",
            "fetchTotalTag",
            "getFacultiesDetails",
            "uploadSubject",
            "deleteSubject"
        ]),
        onSubmit() {
            this.$refs.observer.validate().then(success => {
                if (success) {
                    var subjectFormData = new FormData();
                    subjectFormData.append("id", this.subjectDto.id);
                    subjectFormData.append("name", this.subjectDto.name);
                    subjectFormData.append("year", this.subjectDto.year);
                    subjectFormData.append("file", this.subjectDto.file);
                    subjectFormData.append("type", this.subjectDto.type);
                    subjectFormData.append(
                        "facultyId",
                        this.subjectDto.facultyId
                    );
                    subjectFormData.append(
                        "semesterId",
                        this.subjectDto.semesterId
                    );
                    if( this.subjectDto.sectionId != null)
                      subjectFormData.append(
                        "sectionId",
                        this.subjectDto.sectionId
                    );
                    subjectFormData.append(
                        "description",
                        this.subjectDto.description
                    );

                    if (this.subjectDto.imagePath) {
                        subjectFormData.append(
                            "imagePath",
                            this.subjectDto.imagePath
                        );
                    }
                    this.subjectDto.tagIds.forEach((tagId, index) => {
                        subjectFormData.append(
                            "tagIds[" + index + "]",
                            tagId
                        );
                    });
                    this.subjectDto.doctorsId.forEach((doctorId, index) => {
                        subjectFormData.append(
                            "tagIds[" + (this.subjectDto.tagIds.length + index) + "]",
                            doctorId
                        );
                    });
                    this.uploadSubject({
                        id: this.subjectDto.id,
                        formData: subjectFormData
                    });
                }
            });
        }
    }
};
</script>
