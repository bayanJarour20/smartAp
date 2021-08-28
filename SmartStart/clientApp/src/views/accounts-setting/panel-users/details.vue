<template>
    <div>
        <ValidationObserver ref="observerDateailsPanel">
            <b-card no-body>
                <b-card-header>
                    <strong class="mr-auto">
                        <unicon
                            class="mr-1"
                            name="user"
                            width="18"
                            height="18"
                        ></unicon>
                        معلومات المستخدم
                    </strong>
                    <div class="d-flex align-items-center">
                        <label class="m-0 mr-50">حالة الحساب</label>
                        <b-badge
                            pill
                            :variant="
                                accountDto.dateBlocked ? 'danger' : 'success'
                            "
                            >{{
                                accountDto.dateBlocked ? "غير مفعل" : "مفعل"
                            }}</b-badge
                        >
                    </div>
                </b-card-header>
                <b-card-body class="p-50">
                    <b-col>
                        <b-row>
                            <b-col cols="12" md="6">
                                <EKInputText
                                    :rules="[
                                        {
                                            type: 'required',
                                            message: 'الاسم الكامل مطلوب'
                                        }
                                    ]"
                                    label="الاسم الكامل"
                                    placeholder="ادخل الاسم الكامل"
                                    name="fullname"
                                    v-model="accountDto.name"
                                />
                            </b-col>
                            <b-col cols="12" md="6">
                                <EKInputText
                                    :rules="[
                                        {
                                            type: 'required',
                                            message: 'اسم المستخدم مطلوب'
                                        },
                                        {
                                            type: 'no_spaces',
                                            message: 'لايجب أن يحتوي على فراغات'
                                        }
                                    ]"
                                    label="اسم المستخدم"
                                    placeholder="ادخل اسم المستخدم"
                                    name="username"
                                    v-model="accountDto.userName"
                                />
                            </b-col>
                            <b-col cols="12" md="6" lg="4">
                                <EKInputText
                                    :rules="[
                                        {
                                            type: 'required',
                                            message: 'رقم الهاتف مطلوب'
                                        },
                                        {
                                            type: 'digits:10',
                                            message:
                                                'يجب أن يكون رقم الهاتف عشرة أرقام'
                                        }
                                    ]"
                                    label="رقم الهاتف"
                                    placeholder="ادخل رقم الهاتف"
                                    name="numbername"
                                    v-model="accountDto.phoneNumber"
                                />
                            </b-col>
                            <b-col cols="12" md="6" lg="4">
                                <EKInputText
                                    :rules="[
                                        {
                                            type: 'required',
                                            message: 'بريد الإلكتروني مطلوب'
                                        },
                                        {
                                            type: 'email',
                                            message:
                                                'يجب أن يكون بريد إلكتلروني'
                                        }
                                    ]"
                                    label="بريد الإلكتروني"
                                    placeholder="ادخل بريد الإلكتروني"
                                    name="emailname"
                                    v-model="accountDto.email"
                                />
                            </b-col>
                            <b-col cols="12" md="6" lg="4">
                                <EKInputText
                                    label="كلمة السر"
                                    :rules="[
                                        {
                                            type: 'min:4',
                                            message: 'لايجب أن يقل عن أربعة'
                                        }
                                    ]"
                                    placeholder="ادخل كلمة السر"
                                    name="password"
                                    v-model="accountDto.password"
                                />
                            </b-col>
                            <b-col cols="12" md="6" lg="4">
                                <EKInputPicker
                                    label="تاريخ الميلاد"
                                    :rules="[
                                        {
                                            type: 'required',
                                            message: 'تاريخ الميلاد مطلوب'
                                        }
                                    ]"
                                    name="brithday"
                                    placeholder="ادخل تاريخ الميلاد"
                                    v-model="accountDto.birthday"
                                >
                                </EKInputPicker>
                            </b-col>
                            <b-col cols="12" md="6" lg="4">
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
                            </b-col>
                           
                            <b-col cols="6" md="6" lg="4" >
                                <b-form-group label="الجنس">
                                    <div class="d-flex align-items-center">
                                        <label class="m-0">ذكر</label>
                                        <b-form-checkbox
                                            switch
                                            v-model="accountDto.gender"
                                        ></b-form-checkbox>
                                        <label class="m-0">أنثى</label>
                                    </div>
                                </b-form-group>
                            </b-col>
                            <b-col cols="12">
                                <EKInputTextarea
                                    v-model="accountDto.address"
                                    label=" العنوان"
                                    placeholder="ادخل رقم العنوان"
                                    name="address"
                                />
                            </b-col>
                        </b-row>
                    </b-col>
                </b-card-body>
                <b-card-footer class="d-flex">
                    <b-button
                        class="mr-1"
                        type="submit"
                        variant="primary"
                        @click="onSubmit"
                        >تعديل</b-button
                    >
                    <b-button variant="outline-primary" to="/users/2"
                        >تراجع</b-button
                    >
                    <b-button
                        class="ml-auto"
                        :variant="
                            accountDto.dateBlocked
                                ? 'outline-success'
                                : 'outline-warning'
                        "
                        @click="blockAccount(accountDto.id)"
                        >{{
                            accountDto.dateBlocked ? "إلغاء الحظر" : "حظر"
                        }}</b-button
                    >
                    <b-button
                        class="ml-1"
                        variant="outline-danger"
                        @click="deleteAccount(accountDto.id)"
                    >
                        حذف
                    </b-button>
                </b-card-footer>
            </b-card>
        </ValidationObserver>
    </div>
</template>
<style></style>
<script>
import EKInputText from "@Ekcore/components/EK-forms/EK-input-text";
import EKInputPicker from "@Ekcore/components/EK-forms/EK-input-picker";
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";
import { mapState, mapActions } from "vuex";
import { ValidationObserver } from "vee-validate";
import EKInputTextarea from "@Ekcore/components/EK-forms/EK-input-textarea";

export default {
    components: {
        ValidationObserver,
        EKInputText,
        EKInputPicker,
        EKInputSelect,
        EKInputTextarea
    },
    computed: {
        ...mapState({
            accountDto: state => state.accounts.accountDto,
            faculties: state => state.faculties.faculties,
            rolesList: state => state.accounts.rolesList,
        })
    },
    props: {
        id: String
    },
    created() {
        this.getRoles();
        this.getFacultiesDetails();
        this.accountDetails(this.id);
    },
    methods: {
        ...mapActions([
            "getRoles",
            "getFacultiesDetails",
            "blockAccount",
            "accountDetails",
            "deleteAccount",
            "updateAccount"
        ]),
        onSubmit() {
            this.$refs.observerDateailsPanel.validate().then(success => {
                if (success) {
                    this.updateAccount(this.accountDto);
                }
            });
        }
    }
};
</script>
