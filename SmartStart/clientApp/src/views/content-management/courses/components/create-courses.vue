<template>
    <ValidationObserver ref="observer">
        <b-form>
            <EKDialog
                :title="!id ? 'إضافة دورة' : 'تعديل الدورة'"
                ref="courseDialog"
                @open="$store.commit('Reset_Course_Dto', id)"
                @ok="onSubmit"
                @search="search"
            >
                <template slot="activator">
                    <activaitor
                        @search="search"
                        :placeholder="'ابحث عن دورة محددة'"
                    >
                     
                        <template slot="default">
                            <b-button
                                size="sm"
                                variant="primary"
                                class="text-nowrap"
                                @click="open()"
                            >
                                {{ !id ? "دورة جديد" : "تعديل الدورة" }}
                                <unicon
                                    v-if="!id"
                                    class="ml-1"
                                    name="plus"
                                    width="18"
                                    height="18"
                                    fill="#fff"
                                />
                            </b-button>

                            <b-button
                                v-if="id"
                                size="sm"
                                variant="primary"
                                class="text-nowrap ml-2"
                                @click="goToAddQuestion"
                            >
                                سؤال جديد
                                <unicon
                                    class="ml-1"
                                    name="plus"
                                    width="18"
                                    height="18"
                                    fill="#fff"
                                />
                            </b-button>
                        </template>
                    </activaitor>
                </template>
                <template #body>
                    <EKInputText
                        :rules="[
                            { type: 'required', message: 'اسم الدورة إجباري' }
                        ]"
                        label="اسم الدورة"
                        v-model="courcesDto.name"
                        placeholder="ادخل اسم الدورة"
                        name="name"
                    />
                    <EKInputSelect
                        v-model="courcesDto.year"
                        label="سنة الدورة"
                        placeholder="ادخل سنة الدورة"
                        :rules="[
                            { type: 'required', message: 'سنة الدورة إجباري' }
                        ]"
                        :options="years"
                        name="years"
                    />
                    <EKInputSelect
                        label="المادة"
                        placeholder="اختر المادة"
                        :rules="[
                            {
                                type: 'required',
                                message:
                                    'أدخل المادة التي تكون المادة تابعة  لها'
                            }
                        ]"
                        :options="subjectsList"
                        v-model="courcesDto.subjectId"
                        name="subjectId"
                    />
                    <b-col>
                        <b-row>
                            <EKInputText
                                label="السعر الإفتراضي"
                                v-model="courcesDto.price"
                                placeholder="ادخل السعر الإفتراضي"
                                name="price"
                                type="number"
                                :readonly="courcesDto.isFree"
                            />
                            <div class="text-center">
                                <label class="m-0">مجانية</label>
                                <b-form-checkbox
                                    class="ml-1 mt-1"
                                    v-model="courcesDto.isFree"
                                    switch
                                ></b-form-checkbox>
                            </div>
                        </b-row>
                    </b-col>
                </template>
            </EKDialog>
        </b-form>
    </ValidationObserver>
</template>
<script>
import EKDialog from "@Ekcore/components/EK-dialog";
import EKInputText from "@Ekcore/components/EK-forms/EK-input-text";
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";
import { mapState, mapActions, mapGetters } from "vuex";
import { ValidationObserver } from "vee-validate";
import activaitor from "@Ekcore/components/EK-dialog/activaitor";
export default {
    components: {
        EKDialog,
        activaitor,
        EKInputText,
        EKInputSelect,
        ValidationObserver
    },
    props: {
        id: String
    },
    computed: {
        ...mapGetters(["years"]),
        ...mapState({
            courcesDto: state => state.cources.courcesDto,
            courcesQuestionList: state => state.cources.courcesQuestionList,
            subjectsList: state => state.subjects.subjectsList
        }),
    },
    created() {
        this.fetchTotalTag();
        this.fetchSubject();
    },
    methods: {
        ...mapActions([
            "fetchSubject",
            "fetchTotalTag",
            "addCourse",
            "updateCourse"
        ]),
        onSubmit() {
            this.$refs.observer.validate().then(success => {
                if (success) {
                    if (!this.courcesDto.id) {
                        this.addCourse({
                            name: this.courcesDto.name,
                            year: this.courcesDto.year,
                            type: this.courcesDto.type,
                            price: this.courcesDto.price,
                            isFree: this.courcesDto.isFree,
                            subjectId: this.courcesDto.subjectId,
                        });
                    } else {
                        this.updateCourse({
                            id: this.courcesDto.id,
                            name: this.courcesDto.name,
                            year: this.courcesDto.year,
                            type: this.courcesDto.type,
                            price: this.courcesDto.price,
                            isFree: this.courcesDto.isFree,
                            subjectId: this.courcesDto.subjectId,
                        });
                    }
                }
            });
        },
        goToAddQuestion() {
            this.$router.push(`/questions/1/set/0/${this.id}/${this.courcesQuestionList.subjectId}`)
        },
        open() {
            this.$refs.courseDialog.open();
        },
        search(query) {
            if(!this.id) {
                this.$store.commit('Set_Search_Dto', {
                    keys: ['name'],
                    query
                })
            } else {
                this.$store.commit('Set_Search_Dto', {
                    keys: ['title', 'hint'],
                    query
                })
            }
        },
    }
};
</script>

//   accept=".csv, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel"
