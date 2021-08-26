<template>
    <EKDialog
        title="إضافة مستخدم جديد"
        placeholder="ابحث عن مستخدم محدد"
        btnText="مستخدم جديد"
        @open="$store.commit('Reset_Account_Dto')"
        @ok="onSubmit"
        endClose
        @search="search"
    >
        <template #body>
            <ValidationObserver ref="observer">
                <EKInputText
                    :rules="[{ type: 'required', message: 'اسم الكامل مطلوب' }]"
                    label="  الاسم الكامل"
                    placeholder="ادخل الاسم الكامل"
                    name="name"
                    v-model="accountDto.name"
                />
                <EKInputText
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
                            message: 'اسم المستخدم يجب ان يحوي على احرف انجليزية وأرقام فقط'
                        }
                    ]"
                    name="userName"
                    v-model="accountDto.userName"
                />
                <EKInputText
                    v-model="accountDto.email"
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
                    :rules="[
                        { type: 'required', message: 'كلمة السر مطلوب' },
                        {
                            type: 'min:4',
                            message: 'لايجب أن يقل عن أربعة'
                        }
                    ]"
                    label="  كلمة  السر "
                    placeholder="ادخل كلمة السر "
                    name="password"
                    v-model="accountDto.password"
                />
                <EKInputText
                    v-model="accountDto.phoneNumber"
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
                    v-model="accountDto.birthday"
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
                        <b-form-checkbox switch v-model="accountDto.gender"></b-form-checkbox>
                        <label class="mb-0">انثى</label>
                  </div>
                <EKInputSelect
                        label="السماحية"
                        placeholder="اختر سماحية"
                        :rules="[
                            {
                                type: 'required',
                                message: 'إدخال السماحية مطلوب'
                            }
                        ]"
                        :options="rolesList"
                        name="select"
                        :clearable="true"
                        v-model="accountDto.role"
                    />
                <EKInputTextarea
                    v-model="accountDto.address"
                    label=" العنوان"
                    placeholder="ادخل رقم العنوان"
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
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";
import EKInputTextarea from "@Ekcore/components/EK-forms/EK-input-textarea";
import { mapActions, mapState } from "vuex";

export default {
    components: {
        ValidationObserver,
        EKDialog,
        EKInputText,
        EKInputSelect,
        EKInputPicker,
        EKInputTextarea
    },
    computed: {
        ...mapState({
            accountDto: state => state.accounts.accountDto,
            faculties: state => state.faculties.faculties,
            rolesList: state => state.accounts.rolesList,
        })
    },
    created() {
        this.getFacultiesDetails();
        this.getRoles();
    },
    methods: {
        ...mapActions(["createAccount", "getFacultiesDetails","getRoles"]),
        onSubmit() {
            this.$refs.observer.validate().then(success => {
                if (success) {
                    this.createAccount(this.accountDto);
                }
            });
        },
        search(query) {
            this.$store.commit('Set_Search_Dto', {
                keys: ["userName", "email"],
                query   
            })
        },
    }
};
</script>
