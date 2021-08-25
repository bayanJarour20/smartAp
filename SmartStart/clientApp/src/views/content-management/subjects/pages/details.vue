<template>
    <b-form @submit.prevent="onSubmit">
        <b-card no-body class="mb-2">
            <b-card-header class="align-items-center">
                <h4 class="mr-auto mb-0">تفاصيل المادة</h4>
            </b-card-header>
            <b-card-body>
                <b-card-text>
                    <b-row>
                        <b-col cols="12" md="6">
                            <ValidationObserver ref="observerName">
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
                                    :valueLabel="null"
                                />
                            </ValidationObserver>
                            <ValidationObserver ref="observerSubjectList">
                                <EKInputSelect
                                    v-model="dto.year"
                                    :rules="[
                                        {
                                            type: 'required',
                                            message: 'سنة المادة إجباري'
                                        }
                                    ]"
                                    label="سنة المادة"
                                    placeholder="ادخل سنة المادة"
                                    name="year"
                                    :valueLabel="null"
                                    :options="subjectYear"
                                />
                                <EKInputSelect
                                    v-model="dto.faculty"
                                    label="الكلية"
                                    placeholder="اختر الكلية"
                                    :valueLabel="null"
                                    :rules="[
                                        {
                                            type: 'required',
                                            message:
                                                ' أدخل الكلية التي تكون المادة التابعة لها'
                                        }
                                    ]"
                                    :options="faculties.filter((fac) => subjectDto.subjectFaculties.findIndex((sf) => sf.req.facultyId == fac.id) == -1)"
                                    name="facultyId"
                                />
                                <EKInputSelect
                                    v-model="dto.semester"
                                    label="الفصل"
                                    :valueLabel="null"
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
                                    v-model="dto.section"
                                    label="القسم"
                                    placeholder="قسم عام"
                                    :valueLabel="null"
                                    :rules="[
                                        {
                                            type: 'required',
                                            message: 'قسم المادة إجبارية'
                                        }
                                    ]"
                                    :options="sections"
                                    name="sectionId"
                                    :clearable="true"
                                />
                                <EKInputText
                                    label="سعر المادة"
                                    placeholder="ادخل سعر المادة"
                                    :rules="[
                                        {
                                            type: 'required',
                                            message: 'سعر المادة إجبارية'
                                        }
                                    ]"
                                    v-model="dto.price"
                                    name="price"
                                    type="number"
                                />
                                <EKInputSelect
                                    v-model="dto.doctor"
                                    label="مدرسو المادة"
                                    placeholder="اختر مدرسو المادة"
                                    :valueLabel="null"
                                    :options="doctors"
                                    :rules="[
                                        {
                                            type: 'required',
                                            message: 'مدرسو المادة إجبارية'
                                        }
                                    ]"
                                    name="doctorsId"
                                    :clearable="true"
                                />
                                <b-button
                                    variant="primary"
                                    class="w-100 my-1"
                                    @click="addSubjectFactlesDetails"
                                    ><unicon
                                        name="plus"
                                        width="18"
                                        height="18"
                                        fill="#fff"
                                    />
                                </b-button>
                                <small class="text-danger" v-show="subjectDto.subjectFaculties.length == 0">
                                    يجب تحديد كلية واحدة على الاقل
                                </small>
                                <div
                                    v-for="(subject,
                                    index) in subjectDto.subjectFaculties"
                                    :key="index"
                                >
                                    <div class="div-subject d-flex">
                                        <span>
                                            {{subject.show.faculty}} - 
                                            {{subject.show.section}} - 
                                            {{subject.show.semester}} - 
                                            {{subject.show.doctor}} - 
                                            {{subject.show.year}} - 
                                            {{subject.show.price}}
                                        </span>
                                        <unicon
                                            name="times"
                                            width="18"
                                            height="18"
                                            fill="#7367f0"
                                            class="ml-auto"
                                            @click="deleteSubjectIndex(index)"
                                        />
                                    </div>
                                </div>
                            </ValidationObserver>
                        </b-col>
                        <b-col cols="12" md="6">
                            <ValidationObserver ref="observer">
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
                                <EKInputTextarea
                                    v-model="subjectDto.description"
                                    label="شرح المادة"
                                    placeholder="ادخل شرح المادة"
                                    name="description"
                                    :rules="[
                                        {
                                            type: 'required',
                                            message: 'شرح المادة إجبارية'
                                        }
                                    ]"
                                />
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
                                <div class="d-flex justify-content-between">
                                    <div>
                                        <span>عدد الدورات </span>
                                        <div class="div-number">
                                            {{ subjectDto.examCount }}
                                        </div>
                                    </div>
                                    <div>
                                        <span>عدد البنوك </span>
                                        <div class="div-number">
                                            {{ subjectDto.bankCount }}
                                        </div>
                                    </div>
                                    <div>
                                        <span>عدد المجاهر</span>
                                        <div class="div-number">
                                            {{ subjectDto.microscopeCount }}
                                        </div>
                                    </div>
                                    <div>
                                        <span>
                                            عدد المقابلات
                                        </span>
                                        <div class="div-number">
                                            {{ subjectDto.interviewCount }}
                                        </div>
                                    </div>
                                </div>
                            </ValidationObserver>
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
                            @click="deleteSubject(id)"
                            >حذف</b-button
                        >
                    </b-col>
                </b-row>
            </b-card-footer>
        </b-card>
    </b-form>
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
        ...mapGetters(["semester", "doctors", "tagsList", "sections"]),
        ...mapState({
            subjectDto: state => state.subjects.subjectDto,
            faculties: state => state.faculties.faculties,
            subjectYear: state => state.globalStore.subjectYear
        })
    },
    data: () => ({
        dto: {
            faculty: null,
            semester: null,
            section: null,
            doctor: null,
            year: null,
            price: 0
        }
    }),
    created() {
        this.fetchTotalTag();
        this.subjectDetails(this.id);
        this.getFacultiesDetails();
    },
    methods: {
        ...mapActions([
            "subjectDetails",
            "fetchTotalTag",
            "getFacultiesDetails",
            "setSubject",
            "deleteSubject"
        ]),
        addSubjectFactlesDetails() {
            this.$refs.observerSubjectList.validate().then(success => {
                if (success) {
                    this.subjectDto.subjectFaculties.unshift({
                        req: {
                            facultyId: this.dto.faculty.id,
                            semesterId: this.dto.semester.id,
                            sectionId: this.dto.section.id,
                            doctorId: this.dto.doctor.id,
                            year: this.dto.year.id,
                            price: this.dto.price
                        },
                        show: {
                            faculty: this.dto.faculty.name,
                            semester: this.dto.semester.name,
                            section: this.dto.section.name,
                            doctor: this.dto.doctor.name,
                            year: this.dto.year.name,
                            price: this.dto.price
                        }
                    });
                }
                Object.assign(this.dto, {
                faculty: null,
                semester: null,
                section: null,
                doctor: null,
                year: null,
                price: 0
            })
            });
        
            
        },
        deleteSubjectIndex(i) {
            this.subjectDto.subjectFaculties.splice(i, 1);
        },
        onSubmit() {
            this.$refs.observerName.validate().then(success => {
                if (success) {
                    this.$refs.observer.validate().then(success => {
                        if (
                            success &&
                            this.subjectDto.subjectFaculties.length != 0
                        ) {
                            var subjectFormData = new FormData();
                            subjectFormData.append("id", this.subjectDto.id);
                            subjectFormData.append(
                                "name",
                                this.subjectDto.name
                            );
                            subjectFormData.append(
                                "file",
                                this.subjectDto.file
                            );
                            subjectFormData.append(
                                "type",
                                this.subjectDto.type
                            );
                            subjectFormData.append(
                                "isFree",
                                this.subjectDto.isFree
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
                            this.subjectDto.subjectFaculties.forEach(
                                (subjectFaculties, index) => {
                                    subjectFormData.append(
                                        "subjectFaculties[" +
                                            index +
                                            "].FacultyId",
                                        subjectFaculties.req.facultyId
                                    );
                                    subjectFormData.append(
                                        "subjectFaculties[" +
                                            index +
                                            "].SemesterId",
                                        subjectFaculties.req.semesterId
                                    );
                                    subjectFormData.append(
                                        "subjectFaculties[" +
                                            index +
                                            "].SectionId",
                                        subjectFaculties.req.sectionId
                                    );

                                    subjectFormData.append(
                                        "subjectFaculties[" +
                                            index +
                                            "].DoctorId",
                                        subjectFaculties.req.doctorId
                                    );

                                    subjectFormData.append(
                                        "subjectFaculties[" + index + "].year",
                                        subjectFaculties.req.year
                                    );
                                    subjectFormData.append(
                                        "subjectFaculties[" + index + "].price",
                                        subjectFaculties.req.price
                                    );
                                }
                            );
                            this.setSubject({
                                id: this.subjectDto.id,
                                formData: subjectFormData
                            });
                        }
                    });
                }
            });
        }
    }
};
</script>
<style>
.div-subject {
    border: 1px solid #7367f0;
    border-radius: 25px;
    padding: 12px 16px;
    margin: 16px 0px;
    color: #7367f0;
}
.div-number {
    text-align: center;
    background-color: #7367f0;
    color: #fff;
    border-radius: 20px;
    padding: 4px;
    margin-top: 10px;
}
</style>
