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
                    <EKInputText
                        label="سعر البنك"
                        placeholder="ادخل سعر البنك"
                        :rules="[
                            {
                                type: 'required',
                                message: 'سعر البنك إجباري'
                            },
                            {
                                type: 'min_value:0',
                                message: 'سعر البنك قيمة موجبة'
                            },
                        ]"
                        v-model="bankDto.price"
                        type="number"
                        name="price"
                    />
                    <EKInputSelect
                        label="تصنيفات البنك"
                        placeholder="اختر التصنيف"
                        :rules="[
                            {
                                type: 'required',
                                message: 'تصنيفات البنك إجبارية'
                            }
                        ]"
                        multiple
                        :options="tagsList"
                        v-model="bankDto.tagIds"
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
        ...mapGetters(["tagsList", "years"]),
        ...mapState({
            bankDto: state => state.banks.bankDto,
            subjectsList: state => state.subjects.subjectsList
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
                            price: +this.bankDto.price,
                            isFree: this.bankDto.isFree,
                            subjectId: this.bankDto.subjectId,
                            tagIds: this.bankDto.tagIds,
                        });
                    } else {
                        this.updateBank({
                            id: this.bankDto.id,
                            name: this.bankDto.name,
                            year: this.bankDto.year,
                            type: this.bankDto.type,
                            price: +this.bankDto.price,
                            isFree: this.bankDto.isFree,
                            subjectId: this.bankDto.subjectId,
                            tagIds: this.bankDto.tagIds,
                        });
                    }
                }
            });
        },
        goToAddQuestion() {
            this.$router.push('/questions/1/set/0/0/0')
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
