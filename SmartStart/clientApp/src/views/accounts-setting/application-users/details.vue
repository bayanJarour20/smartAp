<template>
    <div>
        <b-card no-body>
            <b-card-header>
                <strong>
                    <unicon name="user" width="18" height="18"></unicon>
                    معلومات المستخدم
                </strong>
                <span class="ml-auto">
                    حالة الحساب
                 <b-badge pill :variant="userDto.dateBlocked ? 'danger' : 'success'">{{userDto.dateBlocked ? 'غير مفعل' : 'مفعل'}}</b-badge>
                </span>
            </b-card-header>
            <b-card-body class="p-0">
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
                                v-model="userDto.name"
                            />
                            <div style="margin-top:25px">
                            
                            </div>
                        </b-col>
                      
                        <b-col cols="12" md="6">
                            <EKInputText
                                readonly
                                label="تاريخ الإشتراك"
                                name="dateActivated"
                                :value="new Date(userDto.dateActivated).toLocaleDateString('en-GB')"
                            />
                        </b-col>
                        <b-col cols="12" md="6">
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
                                name="numberName"
                                v-model="userDto.phoneNumber"
                            />
                        </b-col>
                        <b-col cols="12" md="6">
                            <EKInputText
                                label="كلمة السر"
                                placeholder="ادخل كلمة السر"
                                name="passwordName"
                                v-model="userDto.password"
                                :rules="[
                                    {
                                        type: 'min:4',
                                        message: 'لايجب أن يقل عن أربعة'
                                    }
                                ]"
                            />
                        </b-col>
                        <b-col cols="12" md="6">
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
                                v-model="userDto.birthday"
                            />
                        </b-col>
                        <b-col cols="12" md="6" lg="3">
                            <label class="mb-50">الجنس</label>
                            <div class="d-flex align-items-center my-1">
                                <label class="mb-0">ذكر</label>
                                <b-form-checkbox switch v-model="userDto.gender"></b-form-checkbox>
                                <label class="mb-0">انثى</label>
                            </div>
                        </b-col>
                        <b-col cols="12" md="6" lg="3">
                            <EKInputText
                                readonly
                                :value="userDto.codes.length"
                                label="عدد الإشتراكات"
                                name="subscriptionCount"
                                type="number"
                            />
                        </b-col>
                        <b-col cols="12">
                            <EKInputTextarea
                                v-model="userDto.address"
                                label=" العنوان"
                                placeholder="ادخل رقم العنوان"
                                name="address"
                            />
                        </b-col>
                    </b-row>
                </b-col>
            </b-card-body>
            <b-card-footer>
                <b-row>
                    <b-col>
                        <div class="d-flex">
                            <b-button
                                class="mr-1"
                                type="submit"
                                variant="primary"
                                style="max-width:100px"
                                @click="updateUser(userDto)"
                                >تعديل</b-button
                            >
                            <b-button
                                variant="outline-primary"
                                style="max-width:100px"
                                to="/users/0"
                                >تراجع</b-button
                            >
                        </div>
                    </b-col>
                    <b-col class="d-flex justify-content-end">
                        <b-button
                            class="ml-auto"
                            :variant="
                                userDto.dateBlocked
                                    ? 'outline-success'
                                    : 'outline-warning'
                            "
                            @click="blockUser(id)"
                            >{{
                                userDto.dateBlocked ? "إلغاء الحظر" : "حظر"
                            }}</b-button
                        >
                        <b-button
                            class="ml-1"
                            variant="outline-danger"
                            @click="deleteUser(id)"
                        >
                            حذف
                        </b-button>
                    </b-col>
                </b-row>
            </b-card-footer>
        </b-card>
        <h2 class="pb-2">
            <unicon width="18" height="18" name="qrcode-scan"></unicon>
            الاشتراكات({{userDto.codes ? userDto.codes.length : '0'}})
            
        </h2>
        <EKTable :items="userDto.codes" :columns="columns" selectedLabel="id"  @delete-selected="UserList" >
            <template slot="items.userName" slot-scope="{ value }">
                {{value ? value : '---' }}
            </template>
            <template slot="items.dateActivated" slot-scope="{ value }">
                <b-badge :variant="value ? 'success' : 'warning'">{{
                    value ? " مفعل" : " غير مفعل"
                }}</b-badge>
            </template>
            <template slot="items.maxEndDate" slot-scope="{ value }">
                {{new Date("0001-01-01T00:00:00").getTime() == new Date(value).getTime() ? '---' : new Date(value).toLocaleDateString("en-GB")}}
            </template>
        </EKTable>
    </div>
</template>
<script>
import EKInputText from "@Ekcore/components/EK-forms/EK-input-text";
import EKInputPicker from "@Ekcore/components/EK-forms/EK-input-picker";
import EKTable from "@Ekcore/components/EK-table";
import { mapActions, mapState } from "vuex";
import EKInputTextarea from "@Ekcore/components/EK-forms/EK-input-textarea";
export default {
    components: {
        EKInputText,
        EKInputPicker,
        EKTable,
        EKInputTextarea
    },
    computed: {
        ...mapState({
            faculties: state => state.faculties.faculties,
            userDto: state => state.accounts.userDto
        })
    },
    props: {
        id: String
    },
    data: () => ({
        columns: [
            {
                label: "الرمز",
                field: "hash"
            },
           
            {
                label: "الحزمة",
                field: "package.name"
            },
            {
                label: "تاريخ نهاية الإشتراك",
                field: "maxEndDate",
                sortable: false
            },
            {
                label: "القيمة المدفوعة",
                field: "value"
            },
            {
                label: "حالة الإشتراك",
                field: "dateActivated",
                sortable: false
            },
            {
                label: "اسم المندوب",
                field: "sellerName"
            }
        ]
    }),
    created() {
        this.userDetails(this.id);
        this.getFacultiesDetails();
    },
    methods: {
        ...mapActions([
            "userDetails",
            "updateUser",
            "blockUser",
            "deleteUser",
            "getFacultiesDetails",
            "UserListDto"
        ]),
        UserList(list){
            this.UserListDto(list)
        }
    }
};
</script>
