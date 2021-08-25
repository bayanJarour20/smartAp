<template>
    <b-form>
        <EKDialog
            title="إضافة مادة"
            placeholder="ابحث عن مادة محددة"
            btnText="مادة جديد"
            @ok="onSubmit"
            @open="$store.commit('Reset_Subject_Dto')"
            @search="search"
            ref="subjectModel"
            class="aside-subject-dialog"
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
                        :options="
                            faculties.filter(
                                fac =>
                                    subjectDto.subjectFaculties.findIndex(
                                        sf => sf.req.facultyId == fac.id
                                    ) == -1
                            )
                        "
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
                    <small class="text-danger" v-show="subjectDto.subjectFaculties.length == 0">
                        يجب تحديد كلية واحدة على الاقل
                    </small>
                     <div
                        v-for="(subject, index) in subjectDto.subjectFaculties"
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
                                @click="deleteSubject(index)"
                            />
                        </div>
                    </div>
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
        ...mapActions(["setSubject", "getFacultiesDetails"]),
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
                            this.$refs.subjectModel.close();
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
                });
            });
        }
    }
};
</script>
<style>
.aside-subject-dialog .b-sidebar {
    width: 500px;
}
.div-subject {
    border: 1px solid #7367f0;
    border-radius: 25px;
    padding: 12px 16px;
    margin: 16px 0px;
    color: #7367f0;
}
</style>
