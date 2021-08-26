<template>
    <ValidationObserver ref="observertelescope">
        <b-form>
            <EKDialog
                ref="telescopeDialog"
                @ok="onSubmit()"
                title="إضافة مجهر"
                placeholder="ابحث عن مجهر محددة"
                btnText="مجهر جديد"
                @open="$store.commit('Reset_Telescope_Dto')"
                @search="search"
            >
                <template #body>
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
                        v-model="telescopeDto.subjectId"
                        name="subjectId"
                    />
                    <EKInputSelect
                        label="السنة"
                        placeholder="اختر السنة"
                        :rules="[
                            {
                                type: 'required',
                                message: 'السنة إجبارية'
                            }
                        ]"
                        :options="years"
                        v-model="telescopeDto.year"
                        name="year"
                    />
                    <EKInputText
                        :rules="[
                            {
                                type: 'required',
                                message: 'اسم المجهر إجباري'
                            }
                        ]"
                        label="اسم المجهر"
                        placeholder="ادخل اسم المجهر"
                        name="name"
                        v-model="telescopeDto.name"
                    />
                    <EKInputText
                        :rules="[
                            {
                                type: 'required',
                                message: 'السعر الإفتراضي إجباري'
                            },
                            {
                                type: 'min_value:0',
                                message: 'الحقل يجب ان يكون بقيمة موجبة'
                            }
                        ]"
                        label="السعر الإفتراضي"
                        placeholder="ادخل السعر الإفتراضي"
                        name="price"
                        type="number"
                        v-model="telescopeDto.price"
                    />
                    <EKInputSelect
                        label="تصنيف المجهر"
                        placeholder="اختر تصنيفات"
                        :rules="[
                            {
                                type: 'required',
                                message: 'التصنيف إجباري'
                            }
                        ]"
                        :options="tagsList"
                        v-model="telescopeDto.tags"
                        multiple
                        name="tags"
                    />
                    <EKInputSelect
                        label="دكتور المجهر"
                        placeholder="اختر الدكتور"
                        :rules="[
                            {
                                type: 'required',
                                message: 'الدكتور إجباري'
                            }
                        ]"
                        multiple
                        :options="doctors"
                        v-model="telescopeDto.doctors"
                        name="doctor"
                    />
                </template>
            </EKDialog>
        </b-form>
    </ValidationObserver>
</template>
<script>
import { ValidationObserver } from "vee-validate";
import EKDialog from "@Ekcore/components/EK-dialog";
import EKInputText from "@Ekcore/components/EK-forms/EK-input-text";
import { mapActions, mapState, mapGetters } from "vuex";
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";
export default {
    components: {
        ValidationObserver,
        EKDialog,
        EKInputText,
        EKInputSelect
    },
    computed: {
        ...mapState({
            telescopeDto: state => state.telescope.telescopeDto,
            subjectsList: state => state.subjects.subjectsList
        }),
        ...mapGetters(["years", "tagsList", "doctors"])
    },
    created() {
        this.fetchSubject();
        this.fetchTotalTag();
    },
    methods: {
        ...mapActions(["fetchSubject", "fetchTotalTag", "addMicroscope"]),
        onSubmit() {
            this.$refs.observertelescope.validate().then(success => {
                if (success) {
                    this.addMicroscope({
                        name: this.telescopeDto.name,
                        year: this.telescopeDto.year,
                        type: this.telescopeDto.type,
                        subjectId: this.telescopeDto.subjectId,
                        price: this.telescopeDto.price,
                        isFree: this.telescopeDto.isFree,
                        tagIds: [...this.telescopeDto.doctors, ...this.telescopeDto.tags]
                    });
                    this.$refs.telescopeDialog.close()
                }
            });
        },
        search(query) {
            this.$store.commit('Set_Search_Dto', {
                keys: ["name", "subject.name"],
                query
            })
        }
    }
};
</script>
