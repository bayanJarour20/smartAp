<template>
    <ValidationObserver ref="observerGenerateCode">
        <EKDialog
            title="إضافة رمز"
            placeholder="ابحث عن رمز محددة"
            btnText="رمز جديد"
            @ok="submit"
            endClose
            @search="search"
        >
            
            <template #body>
                {{codeDto}}
                <EKInputSelect
                    label="الحزمة"
                    placeholder="اختر الحزمة"
                    :rules="[
                        {
                            type: 'required',
                            message: 'اسم الحزمة مطلوب'
                        }
                    ]"
                    v-model="codeDto.packageIds"
                    :options="packagesAvaliableList"
                    name="packageIds"
                    :clearable="true"
                />
                <EKInputText
                    :value="codeDto.packageIds ? new Date(packagesAvaliableList.find(pack => pack.id == codeDto.packageIds).endDate).toLocaleDateString('en-GB') : ''"
                    label="تاريخ النهاية"
                    readonly
                    name="endDate"
                    placeholder="yyyy-mm-dd"
                />
                <EKInputText
                    :value="codeDto.packageIds ? packagesAvaliableList.find(pack => pack.id == codeDto.packageIds).price : ''"
                    label="الكلفة"
                    readonly
                    placeholder="00"
                    type="number"
                    name="price"
                />
                <EKInputText
                    :rules="[
                        { type: 'min_value:0', message: 'يجب ان تكون القيمة موجبة' },
                        { type: 'max_value:100', message: 'لا يجب أن تتجاوز القيمة العدد 100' }
                    ]"
                    label="الحسم"
                    placeholder="0%"
                    type="number"
                    v-model="codeDto.discountRate"
                    name="packageDiscountRate"
                />
            </template>
        </EKDialog>
    </ValidationObserver>
</template>
<script>
import EKInputText from "@Ekcore/components/EK-forms/EK-input-text";
import EKDialog from "@Ekcore/components/EK-dialog";
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";
import { mapState, mapActions, mapGetters } from 'vuex';
import { ValidationObserver } from "vee-validate";

export default {
    components: {
        EKDialog,
        EKInputText,
        EKInputSelect,
        ValidationObserver
    },
    computed: {
        ...mapState({
            codeDto: state => state.codes.codeDto
        }),
        ...mapGetters(["packagesAvaliableList"])
    },
    created() {
        this.getAllPackages()
    },
    methods: {
        ...mapActions(["getAllPackages", "GenerateCode"]),
        submit() {
            this.$refs.observerGenerateCode.validate().then(success => {
                if (success) {
                    this.GenerateCode({
                        discountRate: this.codeDto.discountRate,
                        packageIds: [this.codeDto.packageIds]
                    })
                }
            });
        },
        search(query) {
            this.$store.commit('Set_Search_Dto', {
                keys: [
                    "sellerName", 'packageName',"hash"
                ],
                query   
            })
        },

    }
};
</script>
