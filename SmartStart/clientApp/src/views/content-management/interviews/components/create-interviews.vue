<template>
    <ValidationObserver ref="observer">
        <b-form>
            <EKDialog
                :title="!id ? 'إضافة سؤال كتابي': 'تعديل السؤال الكتابي'"
                ref="bankDialog"
                @open="$store.commit('Reset_Interviews_Dto', id)"
                @ok="onSubmit"
                endClose
            >
                <template slot="activator">
                    <activaitor @search="search" placeholder="ابحث عن سؤال كتابي محدد">
                        <b-button
                            size="sm"
                            variant="primary"
                            class="text-nowrap"
                            @click="open()"
                        >
                           {{!id ? 'سؤال كتابي جديد' : 'تعديل السؤال الكتابي'}}
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
                    </activaitor>
                </template>
                <template #body>
                    <EKInputText
                        :rules="[
                            { type: 'required', message: 'اسم السؤال الكتابي إجباري' }
                        ]"
                        label="اسم السؤال الكتابي"
                        v-model="interviewsDto.name"
                        placeholder="ادخل اسم السؤال الكتابي"
                        name="name"
                    />
                    <EKInputSelect
                        v-model="interviewsDto.year"
                        label="سنة السؤال الكتابي"
                        placeholder="ادخل سنة السؤال الكتابي"
                        :rules="[
                            { type: 'required', message: 'سنة السؤال الكتابي إجباري' }
                        ]"
                        :options="year"
                        name="year"
                    />
                    <EKInputSelect
                        label="المادة"
                        placeholder="اختر المادة"
                        :rules="[
                            {
                                type: 'required',
                                message: 'المادة إجبارية'
                            }
                        ]"
                        :options="subjectsList"
                        v-model="interviewsDto.subjectId"
                        name="subjectId"
                    />
                    <EKInputText
                        label="سعر السؤال الكتابي"
                        placeholder="ادخل سعر السؤال الكتابي"
                        :rules="[
                            {
                                type: 'required',
                                message: 'سعر السؤال الكتابي إجباري'
                            },
                            {
                                type: 'min_value:0',
                                message: 'سعر السؤال الكتابي قيمة موجبة'
                            },
                        ]"
                        v-model="interviewsDto.price"
                        name="price"
                    />
                    <EKInputSelect
                        label="تصنيفات السؤال الكتابي"
                        placeholder="اختر التصنيف"
                        :rules="[
                            {
                                type: 'required',
                                message: 'تصنيفات السؤال الكتابي إجبارية'
                            }
                        ]"
                        multiple
                        :options="tagsList"
                        v-model="interviewsDto.tagIds"
                        name="tagIds"
                    />
                </template>
            </EKDialog>
        </b-form>
    </ValidationObserver>
</template>
<script>
import EKDialog from "@Ekcore/components/EK-dialog";
import EKInputText from "@Ekcore/components/EK-forms/EK-input-text";
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";
import activaitor from "@Ekcore/components/EK-dialog/activaitor";
import { mapState, mapActions, mapGetters } from "vuex";
import { ValidationObserver } from "vee-validate";
export default {
    components: {
        EKDialog,
        EKInputText,
        EKInputSelect,
        ValidationObserver,
        activaitor
    },
    props: {
        id: String
    },
    computed: {
        ...mapGetters(["tagsList"]),
        ...mapState({
            year: state => state.globalStore.year,
            interviewsDto: state => state.interviews.interviewsDto,
            interviewQuestionList: state => state.interviews.interviewQuestionList,
            subjectsList: state => state.subjects.subjectsList
        }),
       
    },
    created() {
        this.fetchTotalTag();
        this.fetchSubject();
    },
    methods: {
        ...mapActions(["fetchSubject", "fetchTotalTag", "addInterview", "updateInterview"]),
        onSubmit() {
            this.$refs.observer.validate().then(success => {
                if (success) {
                    if (!this.interviewsDto.id) {
                        this.addInterview({
                            name: this.interviewsDto.name,
                            year: this.interviewsDto.year,
                            type: this.interviewsDto.type,
                            price: +this.interviewsDto.price,
                            isFree: this.interviewsDto.isFree,
                            subjectId: this.interviewsDto.subjectId,
                            tagIds: this.interviewsDto.tagIds,
                        });
                    } else {
                        this.updateInterview({
                            id: this.interviewsDto.id,
                            name: this.interviewsDto.name,
                            year: this.interviewsDto.year,
                            type: this.interviewsDto.type,
                            price: +this.interviewsDto.price,
                            isFree: this.interviewsDto.isFree,
                            subjectId: this.interviewsDto.subjectId,
                            tagIds: this.interviewsDto.tagIds,
                        });
                    }
                }
            });
        },
        goToAddQuestion() {
            console.log(this.interviewQuestionList)
            this.$router.push(`/questions/0/set/0/${this.interviewQuestionList.id}/${this.interviewQuestionList.subjectId}`)
        },
        open() {
            this.$refs.bankDialog.open()
        },
        search(query) {
            if(!this.id) {
                this.$store.commit('Set_Search_Dto', {
                    keys: ['name', 'subject.name'],
                    query   
                })
            } else {
                this.$store.commit('Set_Search_Dto', {
                    keys: ['title', 'hint'],
                    query   
                })
            }
        }
    }
};
</script>
