<template>
    <b-form>
        <EKDialog
            title="إضافة مادة"
            placeholder="ابحث عن مادة محددة"
            btnText="مادة جديد"
            @ok="onSubmit"
            @open="$store.commit('Reset_Subject_Dto')"
            @search="search"
        >
            <template #body>
                <ValidationObserver ref="observerName">
                    <div class="d-flex justify-content-center">
                        <span>مجانية</span>
                        <b-form-checkbox
                            switch
                            v-model="subjectDto.isFree"
                        ></b-form-checkbox>
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
                </ValidationObserver>
                <ValidationObserver ref="observerSubjectList">
                    <EKInputSelect
                        v-model="dto.faculty"
                        label="الكلية"
                        placeholder="اختر الكلية"
                        :rules="[
                            {
                                type: 'required',
                                message: 'الكلية إجبارية'
                            }
                        ]"
                        :valueLabel="null"
                        :options="faculties.filter((fac) => subjectDto.subjectFaculties.findIndex((sf) => sf.req.FacultyId == fac.id) == -1)"
                        name="select"
                        :clearable="true"
                    />
                    <EKInputSelect
                        v-model="dto.year"
                        :rules="[
                            { type: 'required', message: 'سنة المادة إجباري' }
                        ]"
                        label="سنة المادة"
                        placeholder="ادخل سنة المادة"
                        name="year"
                        :valueLabel="null"
                        :options="subjectYear"
                    />
                    <EKInputSelect
                        v-model="dto.semester"
                        label="الفصل"
                        placeholder="اختر الفصل"
                        :rules="[{ type: 'required', message: 'الفصل إجباري' }]"
                        :options="semester"
                        name="semesterId"
                        :clearable="true"
                        :valueLabel="null"
                    />
                    <EKInputSelect
                        v-model="dto.doctor"
                        label="مدرسو المادة"
                        placeholder="اختر مدرسو المادة"
                        :options="doctors"
                        name="tagIds"
                        :clearable="true"
                        :valueLabel="null"
                        :rules="[
                            {
                                type: 'required',
                                message: 'مدرسو المادة مطلوبين'
                            }
                        ]"
                    />
                    <EKInputSelect
                        label="القسم"
                        placeholder="اختر القسم"
                        :rules="[{ type: 'required', message: 'القسم إجباري' }]"
                        :options="tagsList"
                        v-model="dto.section"
                        name="tags"
                        :clearable="true"
                        :valueLabel="null"
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
                </ValidationObserver>
                <ValidationObserver ref="observer">
                    <b-form-group label="نوع المادة">
                        <div class="d-flex justify-content-between">
                            <b-form-radio v-model="subjectDto.type" value="0"
                                >نظري</b-form-radio
                            >
                            <b-form-radio v-model="subjectDto.type" value="1"
                                >عملي</b-form-radio
                            >
                        </div>
                    </b-form-group>
                    <b-button
                        variant="primary"
                        class="my-1"
                        block
                        @click="addSubjectFactles"
                        ><unicon
                            name="plus"
                            width="18"
                            height="18"
                            fill="#fff"
                        />
                    </b-button>
                    <div
                        v-for="(subject, index) in subjectDto.subjectFaculties"
                        :key="index"
                    >
                        <div class="div-subject">
                            {{ subject.show.faculty }} -{{
                                subject.show.section
                            }}-{{ subject.show.semester }}
                            <unicon
                                name="times"
                                width="18"
                                height="18"
                                fill="#7367f0"
                                @click="deleteSubject(index)"
                            />
                        </div>
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
                        :rules="[
                            {
                                type: 'required',
                                message: 'وصف المادة إجبارية'
                            }
                        ]"
                    />
                </ValidationObserver>
            </template>
        </EKDialog>
    </b-form>
</template>
<script>
import { ValidationObserver } from "vee-validate";
import EKDialog from "@Ekcore/components/EK-dialog";
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";
import EKInputImage from "@Ekcore/components/EK-forms/EK-input-image";
import { mapActions, mapState, mapGetters } from "vuex";
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
        dto: {
            faculty: null,
            semester: null,
            section: null,
            doctor: null,
            year: null,
            price: 0
        }
    }),
    computed: {
        ...mapGetters(["tagsList", "semester", "doctors"]),
        ...mapState({
            subjectDto: state => state.subjects.subjectDto,
            faculties: state => state.faculties.faculties,
            subjectYear: state => state.globalStore.subjectYear
        })
    },
    created() {
        this.getFacultiesDetails();
    },
    methods: {
        ...mapActions(["uploadSubject", "getFacultiesDetails"]),
        onSubmit() {
            this.$refs.observerName.validate().then(success => {
                if (success) {
                    this.$refs.observer.validate().then(success => {
                        if (
                            success &&
                            this.subjectDto.subjectFaculties.length != 0
                        ) {
                            var subjectFormData = new FormData();
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
                                "description",
                                this.subjectDto.description
                            );
                            subjectFormData.append(
                                "isFree",
                                this.subjectDto.isFree
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
                                        subjectFaculties.req.FacultyId
                                    );
                                    subjectFormData.append(
                                        "subjectFaculties[" +
                                            index +
                                            "].SemesterId",
                                        subjectFaculties.req.SemesterId
                                    );
                                    subjectFormData.append(
                                        "subjectFaculties[" +
                                            index +
                                            "].SectionId",
                                        subjectFaculties.req.SectionId
                                    );

                                    subjectFormData.append(
                                        "subjectFaculties[" +
                                            index +
                                            "].DoctorId",
                                        subjectFaculties.req.DoctorId
                                    );

                                    subjectFormData.append(
                                        "subjectFaculties[" + index + "].Year",
                                        subjectFaculties.req.Year
                                    );
                                    subjectFormData.append(
                                        "subjectFaculties[" + index + "].price",
                                        subjectFaculties.req.price
                                    );
                                }
                            );
                            this.uploadSubject({
                                id: this.subjectDto.id,
                                formData: subjectFormData
                            });
                        }
                    });
                }
            });
        },
        deleteSubject(i) {
            this.subjectDto.subjectFaculties.splice(i, 1);
        },
        search(query) {
            this.$store.commit("Set_Search_Dto", {
                keys: ["name"],
                query
            });
        },
        addSubjectFactles() {
            this.$refs.observerSubjectList.validate().then(success => {
                if (success) {
                    this.subjectDto.subjectFaculties.unshift({
                        req: {
                            FacultyId: this.dto.faculty.id,
                            SemesterId: this.dto.semester.id,
                            SectionId: this.dto.section.id,
                            DoctorId: this.dto.doctor.id,
                            Year: this.dto.year.id,
                            price: this.dto.price
                        },
                        show: {
                            faculty: this.dto.faculty.name,
                            semester: this.dto.semester.name,
                            section: this.dto.section.name,
                            doctor: this.dto.doctor.name,
                            Year: this.dto.year.name,
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
           
        }
    }
};
</script>
<style>
.div-subject {
    border: 1px solid #7367f0;
    border-radius: 25px;
    padding: 11px;
    text-align: center;
    margin: 17px 0px;
    color: #7367f0;
}
</style>
