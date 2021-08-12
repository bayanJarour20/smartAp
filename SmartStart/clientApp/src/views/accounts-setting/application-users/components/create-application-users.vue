<template>
    <EKDialog
        title="مستخدم جديدة"
        placeholder="ابحث عن مستخدم جديدة"
        btnText="مستخدم جديدة"
        @ok="onSubmit()"
        @open="$store.commit('Reset_User_Dto')"
        endClose
        @search="search"
    >
        <template #body>
            <ValidationObserver ref="observerUser">
                <EKInputText
                    :rules="[{ type: 'required', message: 'اسم الكامل مطلوب' }]"
                    label="الاسم الكامل"
                    placeholder="ادخل الاسم الكامل"
                    name="full name"
                    v-model="userDto.name"
                />
                <EKInputText
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
                    v-model="userDto.phoneNumber"
                />
                <EKInputText
                    :rules="[{ type: 'required', message: 'اسم المستخدم مطلوب' }]"
                    label="الاسم المستخدم"
                    placeholder="ادخل الاسم المستخدم"
                    name="user name"
                    v-model="userDto.userName"
                />
                <EKInputText
                    :rules="[
                        { type: 'required', message: '  البريد الإلكتروني مطلوب' },
                        {
                            type: 'email',
                            message: 'يجب أن يكون النص بريدا إلكترونيا'
                        }
                    ]"
                    label="البريد الإلكتروني"
                    placeholder="ادخل البريد الإلكتروني "
                    name="email"
                    v-model="userDto.email"
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
                    v-model="userDto.password"
                />            
                <EKInputPicker
                    label="تاريخ الميلاد"
                     
                     
                    name="birthday"
                    placeholder="اختر تاريخ الميلاد"
                    v-model="userDto.birthday"
                />
                <label class="mb-50">الجنس</label>
                <div class="d-flex align-items-center mb-1">
                    <label class="mb-0">ذكر</label>
                    <b-form-checkbox switch v-model="userDto.gender"></b-form-checkbox>
                    <label class="mb-0">انثى</label>
                </div>
                <EKInputTextarea
                    v-model="userDto.address"
                    label=" العنوان"
                    placeholder="ادخل رقم العنوان"
                    name="address"
                />
                <EKInputSelect
                placeholder="اختر كلية محددة "
                :options="faculties"
                name="selectFactilies"
                :clearable="true"
                v-model="userDto.facultyId"
                    />
            </ValidationObserver>
        </template>
    </EKDialog>
</template>
<script>
import EKDialog from "@Ekcore/components/EK-dialog";
import EKInputText from "@Ekcore/components/EK-forms/EK-input-text";
import EKInputPicker from "@Ekcore/components/EK-forms/EK-input-picker";
import { ValidationObserver } from "vee-validate";
import EKInputTextarea from "@Ekcore/components/EK-forms/EK-input-textarea";

import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";

import { mapActions, mapState } from "vuex";

export default {
    components: {
        EKInputSelect,
        EKDialog,
        EKInputText,
        EKInputPicker,
        ValidationObserver,
        EKInputTextarea
    },
    computed: {
        ...mapState({
            userDto: state => state.accounts.userDto,
            faculties: state => state.faculties.faculties
        })
    },
    props: {
        id: String
    },
    created() {
        this.getFacultiesDetails();
    },
    methods: {
        ...mapActions(["createUser", "getFacultiesDetails"]),
        onSubmit() {
            this.$refs.observerUser.validate().then(success => {
                if (success) {
                    this.createUser(this.userDto);
                }
            });
        },
        search(query) {
            this.$store.commit('Set_Search_Dto', {
                keys: [
                    "userName","email","address","name"
                ],
                query   
            })
        },
    }
};
</script>
