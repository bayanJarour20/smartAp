<template>
    <div>
        <ValidationObserver ref="observer">
            <b-card no-body class="mb-2">
                <b-card-header>
                    <strong>
                        <unicon
                            class="mr-1"
                            name="user"
                            width="18"
                            height="18"
                        ></unicon>
                        معلومات نقاط البيع
                    </strong>
                    <div class="ml-auto d-flex mr-1 align-items-center">
                        <label class="m-0">إمكانية الحسم</label>
                        <b-form-checkbox
                            switch
                            v-model="posDto.allowDiscount"
                        ></b-form-checkbox>
                    </div>
                    <div class="d-flex align-items-center">
                        <label class="m-0 mr-50">حالة الحساب</label>
                        <b-badge
                            pill
                            :variant="posDto.dateBlocked ? 'danger' : 'success'"
                            >{{
                                posDto.dateBlocked ? "غير مفعل" : "مفعل"
                            }}</b-badge
                        >
                    </div>
                </b-card-header>
                <b-card-body class="px-2 py-0">
                    <b-row>
                        <b-col cols="12" md="6">
                            <EKInputText
                                v-model="posDto.name"
                                :rules="[
                                    {
                                        type: 'required',
                                        message: 'الاسم الكامل مطلوب'
                                    }
                                ]"
                                label="الاسم الكامل"
                                placeholder="ادخل الاسم الكامل"
                                name="name"
                            />
                        </b-col>
                        <b-col cols="12" md="6">
                            <EKInputText
                                v-model="posDto.userName"
                                :rules="[
                                    {
                                        type: 'required',
                                        message: 'اسم المستخدم مطلوب'
                                    }
                                ]"
                                label="اسم المستخدم"
                                placeholder="ادخل اسم المستخدم"
                                name="userName"
                            />
                        </b-col>
                        <b-col cols="12" md="6">
                            <EKInputText
                                v-model="posDto.phoneNumber"
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
                                name="phoneNumber"
                            />
                        </b-col>
                        <b-col cols="12" md="6">
                            <EKInputText
                                v-model="posDto.email"
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
                            />
                        </b-col>
                        <b-col cols="12" md="6" lg="4">
                            <EKInputText
                                v-model="posDto.password"
                                label="كلمة السر"
                                :rules="[
                                    {
                                        type: 'min:4',
                                        message: 'لايجب أن يقل عن أربعة'
                                    }
                                ]"
                                placeholder="ادخل كلمة السر"
                                name="password"
                            />
                        </b-col>
                        <b-col cols="12" md="6" lg="4">
                            <EKInputPicker
                                v-model="posDto.birthday"
                                label="تاريخ الميلاد"
                                :rules="[
                                    {
                                        type: 'required',
                                        message: 'تاريخ الميلاد مطلوب'
                                    }
                                ]"
                                name="birthday"
                                placeholder="ادخل تاريخ الميلاد"
                            >
                            </EKInputPicker>
                        </b-col>

                        <b-col cols="12" md="6" lg="4">
                            <EKInputText
                                v-model="posDto.moneyLimit"
                                :rules="[
                                    {
                                        type: 'required',
                                        message:
                                            'الحد الاعظمي من المبيعات مطلوب'
                                    }
                                ]"
                                label="الحد المالي الاعظمي"
                                placeholder="ادخل الحد المالي الاعظمي "
                                name="moneyLimit"
                                type="number"
                            />
                        </b-col>
                        <b-col cols="12" md="6" lg="4">
                            <EKInputPicker
                                v-model="posDto.subscriptionDate"
                                label="تاريخ الإشتراك"
                                readonly
                                name="subscriptionDate"
                            >
                            </EKInputPicker>
                        </b-col>
                        <b-col cols="12" md="6" lg="4">
                            <EKInputText
                                v-model="posDto.codeSoldCount"
                                readonly
                                label="عدد الرموز المولدة"
                                name="codeSoldCount"
                                type="number"
                            />
                        </b-col>
                        <b-col cols="12" md="6" lg="4">
                            <label class="mt-1">الجنس</label>
                            <div class="d-flex align-items-center mb-1 mt-50">
                                <label class="mb-0">ذكر</label>
                                <b-form-checkbox
                                    switch
                                    v-model="posDto.gender"
                                ></b-form-checkbox>
                                <label class="mb-0">انثى</label>
                            </div>
                        </b-col>
                        <b-col cols="12" md="6" lg="4">
                            <EKInputSelect
                                label="الكليات التابعة لنقطة البيع"
                                placeholder="كل الكليات"
                                multiple
                                :options="faculties"
                                v-model="posDto.facList"
                                name="facList"
                            />
                        </b-col>
                        <b-col cols="12" md="6" lg="4">
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
                        </b-col>
                        <b-col cols="12" md="6" lg="4">
                            <EKInputText
                                :rules="[
                                    {
                                        type: 'required',
                                        message: 'نسبة نقطة البيع مطلوبة'
                                    },
                                    {
                                        type: 'min_value:0',
                                        message: 'يجب ان تكون القيمة موجبة'
                                    },
                                    {
                                        type: 'max_value:100',
                                        message:
                                            'لا يجب أن تتجاوز القيمة العدد 100'
                                    }
                                ]"
                                label="نسبة نقطة البيع"
                                placeholder="ادخل نسبة نقطة البيع"
                                v-model="posDto.rate"
                                type="number"
                                name="packageDiscountRate"
                            />
                        </b-col>
                        <b-col cols="12">
                            <EKInputTextarea
                                v-model="posDto.address"
                                :rules="[
                                    {
                                        type: 'required',
                                        message: 'العنوان  مطلوب'
                                    }
                                ]"
                                label="العنوان "
                                placeholder="ادخل العنوان "
                                name="address"
                            />
                        </b-col>
                    </b-row>
                </b-card-body>
                <b-card-footer class="d-flex">
                    <b-button class="mr-1" variant="primary" @click="onSubmit"
                        >تعديل</b-button
                    >
                    <b-button variant="outline-primary" to="/users/1"
                        >تراجع</b-button
                    >
                    <b-button
                        class="ml-auto"
                        :variant="
                            posDto.dateBlocked
                                ? 'outline-success'
                                : 'outline-warning'
                        "
                        @click="blockPOS(posDto.id)"
                        >{{
                            posDto.dateBlocked ? "إلغاء الحظر" : "حظر"
                        }}</b-button
                    >
                    <b-button
                        class="ml-1"
                        variant="outline-danger"
                        @click="deletePOS(posDto.id)"
                    >
                        حذف
                    </b-button>
                </b-card-footer>
            </b-card>
        </ValidationObserver>
        <h3 class="mb-2">
            <unicon
                width="18"
                height="18"
                name="qrcode-scan"
                class="mr-1"
            ></unicon>
            الرموز المولدة
        </h3>
        <EKTable
            :items="posDto.codeDetailsSimpleDto"
            :columns="columns"
            selectedLabel="id"
            @delete-selected="CodesOfpointSales"
        >
            <template slot="items.userName" slot-scope="{ value }">
                {{ value ? value : "---" }}
            </template>
            <template slot="items.dateActivated" slot-scope="{ value }">
                <b-badge :variant="value ? 'success' : 'warning'">{{
                    value ? " مفعل" : " غير مفعل"
                }}</b-badge>
            </template>
            <template slot="items.maxEndDate" slot-scope="{ value }">
                {{
                    new Date("0001-01-01T00:00:00").getTime() ==
                    new Date(value).getTime()
                        ? "---"
                        : new Date(value).toLocaleDateString("en-GB")
                }}
            </template>
            <template slot="items.createDate" slot-scope="{ value }">
                {{ moment(new Date(value)).format("YYYY/MM/DD HH:mm:ss") }}
            </template>
            <template slot="items.package" slot-scope="{ value }">
                {{ value.name }}
            </template>
        </EKTable>
    </div>
</template>
<style>
.span-title {
    background-color: rgba(40, 199, 111, 0.12);
    color: #28c76f;
    border-radius: 37%;
    padding: 9px;
}
</style>
<script>
import EKInputText from "@Ekcore/components/EK-forms/EK-input-text";
import { ValidationObserver } from "vee-validate";
import EKInputPicker from "@Ekcore/components/EK-forms/EK-input-picker";
import EKTable from "@Ekcore/components/EK-table";
import { mapActions, mapState } from "vuex";
import EKInputTextarea from "@Ekcore/components/EK-forms/EK-input-textarea";
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";
import moment from "moment";
export default {
    components: {
        ValidationObserver,
        EKInputText,
        EKInputPicker,
        EKTable,
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
    props: {
        id: String
    },
    data: () => ({
        columns: [
            {
                label: "الرموز",
                field: "hash"
            },
            {
                label: "اسم الطالب",
                field: "studentName"
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
                label: "تاريخ الإنشاء",
                field: "createDate",
                sortable: false
            },
            {
                label: "الحزمة",
                field: "package"
            }
        ]
    }),
    created() {
        this.fetchCity();
        this.posDetails(this.id);
        this.getFacultiesDetails();
    },
    methods: {
        moment,
        ...mapActions([
            "posDetails",
            "updatePOS",
            "blockPOS",
            "deletePOS",
            "fetchCity",
            "CodeListDto",
            "CodePointListDto",
            "getFacultiesDetails"
        ]),
        onSubmit() {
            this.$refs.observer.validate().then(success => {
                if (success) {
                    this.updatePOS(this.posDto);
                }
            });
        },
        CodesOfpointSales(list) {
            this.CodePointListDto(list);
        }
    }
};
</script>
