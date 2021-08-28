<template>
    <ValidationObserver ref="observer">
        <b-form>
            <EKDialog
                :title="!id ? 'إضافة بنك': 'تعديل البنك'"
                ref="bankDialog"
                @open="$store.commit('Reset_Bank_Dto', id)"
                endClose
                @ok="onSubmit"
                @search="search"
            >
                <template slot="activator">
                    <activaitor @search="search" :placeholder="!id ? 'ابحث عن بنك محدد' : 'ابحث عن سؤال محدد'">
                        <b-button
                            size="sm"
                            variant="primary"
                            class="text-nowrap"
                            @click="open()"
                        >
                           {{!id ? 'بنك جديد' : 'تعديل البنك'}}
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
                            { type: 'required', message: 'اسم البنك إجباري' }
                        ]"
                        label="اسم البنك"
                        v-model="bankDto.name"
                        placeholder="ادخل اسم البنك"
                        name="name"
                    />
                    <EKInputSelect
                        v-model="bankDto.year"
                        label="سنة البنك"
                        placeholder="ادخل سنة البنك"
                        :rules="[
                            { type: 'required', message: 'سنة البنك إجباري' }
                        ]"
                        :options="years"
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
                        v-model="bankDto.subjectId"
                        name="subjectId"
                    />
                    <EKInputSelect
                        label="تصنيفات البنك"
                        placeholder="اختر تصنيفات"
                        :rules="[
                            {
                                type: 'required',
                                message:
                                    'اختر التصنيفات التي كون البنك تابع لها'
                            }
                        ]"
                        multiple
                        :options="tagsList"
                        v-model="bankDto.categories"
                        name="categories"
                    />
                    <EKInputSelect
                        label="فرق البنك"
                        placeholder="اختر الفرق"
                        :rules="[
                            {
                                type: 'required',
                                message:
                                    'اختر الفرق التي يكون البنك تابع لها'
                            }
                        ]"
                        :options="teams"
                        v-model="bankDto.teams"
                        name="teams"
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
import { mapState, mapActions, mapGetters } from "vuex";
import { ValidationObserver } from "vee-validate";
import activaitor from "@Ekcore/components/EK-dialog/activaitor";
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
        ...mapGetters(["years", "tagsList", "teams"]),
        ...mapState({
            bankDto: state => state.banks.bankDto,
            banksQuestionList: state => state.banks.banksQuestionList,
            subjectsList: state => state.subjects.subjectsList,
        }),
    },
    created() {
        this.fetchTotalTag();
        this.fetchSubject();
    },
    methods: {
        ...mapActions(["fetchSubject", "fetchTotalTag", "addBank", "updateBank"]),
        onSubmit() {
            this.$refs.observer.validate().then(success => {
                if (success) {
                    if (!this.bankDto.id) {
                        this.addBank({
                            name: this.bankDto.name,
                            year: this.bankDto.year,
                            type: this.bankDto.type,
                            subjectId: this.bankDto.subjectId,
                            tagIds: [
                                ...this.bankDto.categories,
                                this.bankDto.teams
                            ]
                        });
                    } else {
                        this.updateBank({
                            id: this.bankDto.id,
                            name: this.bankDto.name,
                            year: this.bankDto.year,
                            type: this.bankDto.type,
                            subjectId: this.bankDto.subjectId,
                            tagIds: [
                                ...this.bankDto.categories,
                                this.bankDto.teams
                            ]
                        });
                    }
                }
            });
        },
        goToAddQuestion() {
            this.$router.push(`/questions/1/set/0/${this.id}/${this.banksQuestionList.subjectId}`)
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
