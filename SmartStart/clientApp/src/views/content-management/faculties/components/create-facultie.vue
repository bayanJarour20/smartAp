<template>
    <ValidationObserver ref="observer">
        <b-form>
            <EKDialog
                ref="facultieDialog"
                @ok="submit"
                :title="title"
                :placeholder="!isEdit ? 'ابحث عن كلية محددة' : ''"
                :btnText="!isEdit ? 'كلية جديد' : ''"
                @close="$store.commit('Set_Facultie_Dto')"
                :isEdit="!!facultyDto.id"
                @delete="deleteFaculty(facultyDto.id)"
                @search="search"
            > 
                <template #body>
                    <EKInputText
                        :rules="[
                            { type: 'required', message: 'اسم الكلية إجباري' }
                        ]"
                        label="اسم الكلية"
                        v-model="facultyDto.name"
                        placeholder="ادخل اسم الكلية"
                        name="name"
                    />
                    <EKInputSelect
                        label="الجامعة"
                        placeholder="اختر جامعة"
                        :rules="[
                            {
                                type: 'required',
                                message: 'يجب تحديد حقل الجامعة'
                            }
                        ]"
                        :options="university"
                        name="university"
                        v-model="facultyDto.universityId"
                        :clearable="true"
                    />
                    <EKInputText
                        :rules="[
                            { type: 'required', message: 'اسم الكلية إجباري' },
                            { type: 'min_value:1', message: 'عدد السنوات يجب ان يكون اكبر او يساوي الواحد' }
                        ]"
                        label="عدد السنوات"
                        v-model="facultyDto.numberOfYear"
                        placeholder="ادخل عدد السنوات"
                        name="numberOfYear"
                        type="number"
                    />
                    <EKInputImage
                        label="صورة الكلية"
                        title="أدرج الملف هنا أو انقر للرفع"
                        @input="facultyDto.file = $event"
                        :value="
                            facultyDto.imagePath
                                ? $store.getters['app/domainHost'] +
                                  '/' +
                                  facultyDto.imagePath
                                : null
                        "
                    >
                    </EKInputImage>
                </template>
            </EKDialog>
        </b-form>
    </ValidationObserver>
</template>
<script>
import { ValidationObserver } from "vee-validate";
import EKInputText from "@Ekcore/components/EK-forms/EK-input-text";
import EKInputImage from "@Ekcore/components/EK-forms/EK-input-image";
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";
import EKDialog from "@Ekcore/components/EK-dialog";
import { mapState, mapActions } from "vuex";
export default {
    components: {
        ValidationObserver,
        EKDialog,
        EKInputText,
        EKInputImage,
        EKInputSelect
    },
    props: {
        title: {
            type: String,
            default: () => "إضافة كلية"
        },
        isEdit: Boolean
    },
    computed: {
        ...mapState({
            facultyDto: state => state.faculties.facultyDto,
            university: state => state.university.universities
        })
    },
    created() {
        this.fetchUniversity();
    },
    methods: {
        ...mapActions(["fetchUniversity", "actionFaculty", "deleteFaculty"]),
        submit() {
            this.$refs.observer.validate().then(success => {
                if (
                    success &&
                    (this.facultyDto.file || this.facultyDto.imagePath)
                ) {
                    var facultyFormData = new FormData();
                    if (!this.facultyDto.id) {
                        facultyFormData.append(
                            "numberOfYear",
                            this.facultyDto.numberOfYear
                        );
                        facultyFormData.append("name", this.facultyDto.name);
                        facultyFormData.append("file", this.facultyDto.file);
                        facultyFormData.append(
                            "universityId",
                            this.facultyDto.universityId
                        );
                    } else {
                        Object.keys(this.facultyDto).forEach(key => {
                            facultyFormData.append(key, this.facultyDto[key]);
                        });
                    }

                    this.actionFaculty({
                        id: this.facultyDto.id,
                        formData: facultyFormData
                    });
                }
            });
        },
        openDialog() {
            this.$refs.facultieDialog.open();
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
