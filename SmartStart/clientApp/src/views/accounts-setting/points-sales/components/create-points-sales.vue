<template>
    <EKDialog
        title=" نقاط  بيع جديدة"
        placeholder="ابحث عن نقاط  بيع جديدة"
        btnText=" نقاط  بيع جديدة"
        @open="$store.commit('Reset_Pos_Dto')"
        @ok="onSubmit"
        @search="search"
        endClose
    >
        <template #body>
            <ValidationObserver ref="observer">
                <EKInputText
                    v-model="posDto.name"
                    :rules="[{ type: 'required', message: 'اسم الكامل مطلوب' }]"
                    label="  الاسم الكامل"
                    placeholder="ادخل الاسم الكامل"
                    name="name"
                />
                <EKInputText
                    v-model="posDto.userName"
                    label="اسم المستخدم"
                    placeholder="ادخل اسم المستخدم"
                    :rules="[
                        {
                            type: 'required',
                            message: 'اسم المستخدم مطلوب'
                        },
                        {
                            type: 'no_spaces',
                            message: 'اسم المستخدم لا يمكن ان يحوي فراغات'
                        },
                        {
                            type: 'english_letters_numbers',
                            message:
                                'اسم المستخدم يجب ان يحوي على احرف انجليزية وأرقام فقط'
                        }
                    ]"
                    name="userName"
                />
                <EKInputText
                    v-model="posDto.email"
                    :rules="[
                        {
                            type: 'required',
                            message: 'البريد الإلكتروني مطلوب'
                        },
                        {
                            type: 'email',
                            message: 'يجب أن يكون بريد إلكتروني وليس نص'
                        }
                    ]"
                    label="  البريد الالكتروني "
                    placeholder="ادخل البريد الالكتروني "
                    name="email"
                />
                <EKInputText
                    v-model="posDto.password"
                    :rules="[{ type: 'required', message: 'كلمة السر مطلوب' }]"
                    label="  كلمة  السر "
                    placeholder="ادخل كلمة السر "
                    name="password"
                />
                <EKInputText
                    v-model="posDto.phoneNumber"
                    :rules="[
                        { type: 'required', message: 'رقم الهاتف مطلوب' },
                        {
                            type: 'digits:10',
                            message: 'يجب أن يكون رقم الهاتف عشرة أرقام'
                        }
                    ]"
                    label=" رقم الهاتف "
                    placeholder="ادخل رقم الهاتف "
                    name="phoneNumber"
                />
                <EKInputPicker
                    v-model="posDto.birthday"
                    label="تاريخ الميلاد"
                    :rules="[
                        { type: 'required', message: 'تاريخ الميلاد مطلوب' }
                    ]"
                    name="birthday"
                    placeholder="اختر تاريخ الميلاد"
                />
                <label class="mb-50">الجنس</label>
                <div class="d-flex align-items-center mb-1">
                    <label class="mb-0">ذكر</label>
                    <b-form-checkbox
                        switch
                        v-model="posDto.gender"
                    ></b-form-checkbox>
                    <label class="mb-0">انثى</label>
                </div>
                <EKInputText
                    v-model="posDto.moneyLimit"
                    :rules="[
                        {
                            type: 'required',
                            message: 'الحد الاعظمي من المبيعات مطلوب'
                        },
                        {
                            type: 'min_value:0',
                            message: 'يجب ان تكون القيمة موجبة'
                        }
                    ]"
                    label="الحد الاعظمي"
                    placeholder="ادخل الحد الاعظمي "
                    name="moneyLimit"
                    type="number"
                />
                <EKInputText
                    :rules="[
                        { type: 'required', message: 'نسبة نقطة البيع مطلوبة' },
                        {
                            type: 'min_value:0',
                            message: 'يجب ان تكون القيمة موجبة'
                        },
                        {
                            type: 'max_value:100',
                            message: 'لا يجب أن تتجاوز القيمة العدد 100'
                        }
                    ]"
                    label="نسبة نقطة البيع"
                    placeholder="ادخل نسبة نقطة البيع"
                    v-model="posDto.rate"
                    type="number"
                    name="packageDiscountRate"
                />
                <EKInputSelect
                    label="الكليات التابعة لنقطة البيع"
                    placeholder="كل الكليات"
                    multiple
                    :options="faculties"
                    v-model="posDto.facIds"
                    name="facIds"
                />
                <b-form-checkbox v-model="posDto.allowDiscount" switch
                    >إمكانية الحسم</b-form-checkbox
                >
                <EKInputSelect
                    label="المدينة"
                    placeholder="اختر المدينة"
                    :options="citiesList"
                    :rules="[
                        {
                            type: 'required',
                            message: 'المدينة مطلوبة'
                        }
                    ]"
                    v-model="posDto.cityId"
                    name="cityId"
                    :clearable="true"
                />

                <EKInputTextarea
                    v-model="posDto.address"
                    :rules="[{ type: 'required', message: 'العنوان مطلوب' }]"
                    label=" العنوان "
                    placeholder="ادخل العنوان "
                    name="address"
                />
            </ValidationObserver>
        </template>
    </EKDialog>
</template>
<script>
import { ValidationObserver } from "vee-validate";
import EKDialog from "@Ekcore/components/EK-dialog";
import EKInputText from "@Ekcore/components/EK-forms/EK-input-text";
import EKInputPicker from "@Ekcore/components/EK-forms/EK-input-picker";
import { mapState, mapActions } from "vuex";
import EKInputTextarea from "@Ekcore/components/EK-forms/EK-input-textarea";
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";
export default {
    components: {
        EKDialog,
        EKInputText,
        EKInputPicker,
        ValidationObserver,
        EKInputTextarea,
        EKInputSelect
    },
    computed: {
        ...mapState({
            citiesList: state => state.globalStore.citiesList,
            posDto: state => state.accounts.posDto,
            faculties: state => state.faculties.faculties
        })
    },
    created() {
        this.fetchCity();
    },
    methods: {
        ...mapActions(["createPOS", "fetchCity"]),
        onSubmit() {
            this.$refs.observer.validate().then(success => {
                if (success) {
                    this.createPOS(this.posDto);
                }
            });
        },
        search(query) {
            this.$store.commit("Set_Search_Dto", {
                keys: ["userName", "email"],
                query
            });
        }
    }
};
</script>
