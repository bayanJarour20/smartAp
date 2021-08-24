<template>
    <b-card no-body class="mb-2">
        <b-card-header class="align-items-center">
            <h4 class="mr-auto mb-0">
                <unicon name="user" width="18" height="18"></unicon>
                معلومات الحزمة
            </h4>
        </b-card-header>
        <ValidationObserver ref="observer">
            <b-card-body class="pt-0">
                <b-card-text>
                    <b-row>
                        <b-col cols="12" md="4">
                            <EKInputText
                                :rules="[
                                    {
                                        type: 'required',
                                        message: 'اسم الحزمة إجباري'
                                    }
                                ]"
                                v-model="packageDto.name"
                                label="اسم الحزمة"
                                placeholder="ادخل اسم الحزمة"
                                name="name"
                            />
                        </b-col>
                        <b-col
                            cols="12"
                            md="4"
                            class="d-flex align-items-center"
                        >
                            <b-checkbox
                                switch
                                v-model="packageDto.type"
                            ></b-checkbox>
                            <label class="mr-auto mb-0">الحزمة عرض خاص</label>
                        </b-col>
                        <b-col
                            cols="12"
                            md="4"
                            class="d-flex align-items-center "
                        >
                            <b-checkbox
                                v-model="packageDto.isHidden"
                                switch
                            ></b-checkbox>
                            <label class="mb-0 mr-auto">حالة الحزمة</label>
                        </b-col>
                        <b-col cols="12">
                            <EKInputPicker
                                label="تاريخ الإنتهاء"
                                v-model="packageDto.endDate"
                                :rules="[
                                    {
                                        type: 'required',
                                        message: 'تاريخ الإنتهاء مطلوب'
                                    }
                                ]"
                                name="endDate"
                                placeholder="اختر تاريخ الإنتهاء"
                            />
                        </b-col>
                        <b-col cols="12" class="my-1">
                            <div style="display: flex; flex-direction: column;">
                                <label>وصف الحزمة</label>
                                <b-textarea
                                    v-model="packageDto.description"
                                ></b-textarea>
                            </div>
                        </b-col>
                    </b-row>
                </b-card-text>
            </b-card-body>
        </ValidationObserver>
        <b-card-header class="flex-column pt-0">
            <h4 class="mr-auto mb-0">
                <unicon name="user" width="18" height="18"></unicon>
                محتويات الحزمة
            </h4>
            <strong>فلترة حسب</strong>
        </b-card-header>
        <b-card-body>
            <ValidationObserver ref="filterTabsFaculity">
                <b-row>
                    <b-col cols="12" md="4">
                        <EKInputSelect
                            label="السنة"
                            placeholder="اختر السنة"
                            :options="year"
                            name="year"
                            :rules="[
                                {
                                    type: 'required',
                                    message: 'السنة مطلوبة'
                                }
                            ]"
                            v-model="filterDto.year"
                            :clearable="true"
                        />
                    </b-col>
                    <b-col cols="12" md="4">
                        <EKInputSelect
                            label="الفصل"
                            placeholder="اختر الفصل "
                            v-model="filterDto.semester"
                            :rules="[
                                {
                                    type: 'required',
                                    message: 'الفصل مطلوب'
                                }
                            ]"
                            :options="semester"
                            name="semester"
                            :clearable="true"
                        />
                    </b-col>
                    <b-col cols="12" md="4">
                        <EKInputSelect
                            label="الكلية"
                            placeholder="اختر الكلية"
                            v-model="filterDto.faculity"
                            :rules="[
                                {
                                    type: 'required',
                                    message: 'الكلية مطلوب'
                                }
                            ]"
                            :options="faculties"
                            name="faculity"
                            :clearable="true"
                        />
                    </b-col>
                    <b-col cols="12" class="text-right">
                        <b-button
                            @click="localeFetchTabsFacultyBy(filterDto)"
                            variant="primary"
                            >فلترة</b-button
                        >
                    </b-col>
                </b-row>
            </ValidationObserver>
        </b-card-body>
        <b-card-body class="py-0">
            <EKTableCollapse
                label="label"
                :rows="activeTabsFaculty"
                :columns="columns"
                no_delete
                childId="id"
                :innerValue="packageDto.selectedExams"
                childrenLabel="exams"
                customHeaderLabel="label"
                :colapseHeader="examHeader"
                @details="details"
                @changeSelectChildren="changeSelectChildren"
            >
                <!-- @changeParentcheck="chaingeSelect" -->
                <template slot="item-td.type" scope="{tr}">
                    <td>
                        {{
                            tr.type == 0
                                ? "دورة"
                                : tr.type == 1
                                ? "بنك"
                                : tr.type == 2
                                ? "مجهر"
                                : "سؤال نصي"
                        }}
                    </td>
                </template>
                <template slot="item-td.price" scope="{parent, trIndex}">
                    <td>
                        <EKInputText
                            @change="calcPackagePrice()"
                            style="width: 120px"
                            class="d-block m-auto"
                            :rules="[
                                {
                                    type: 'required',
                                    message: 'السعر إجباري'
                                },
                                {
                                    type: 'min_value:0',
                                    message: 'السعر مطلوب'
                                }
                            ]"
                            v-model="parent.exams[trIndex].price"
                            type="number"
                            placeholder="ادخل السعر"
                            name="name"
                        />
                    </td>
                </template>
                <template slot="table-footer">
                    <b-card-footer class="px-1 pt-1 pb-0 border-0 text-right">
                        <strong
                            >سعر الحزمة الإجمالي :
                            {{ packageDto.price }}</strong
                        >
                    </b-card-footer>
                </template>
            </EKTableCollapse>
            <b-col cols="12" class="d-flex justify-content-center mb-3">
                <EKPagination :pageLength="5" :items="tabsFaculty" v-model="activeTabsFaculty" />
            </b-col>
            <h6
                class="text-danger text-center mb-2"
                v-if="!packageDto.selectedExams.length"
            >
                يجب اختيار عنصر واحد على الاقل
            </h6>
        </b-card-body>
        <b-card-footer class="py-1">
            <b-button variant="primary" @click="onSubmit()">{{this.id != (0 & "") ? 'تعديل' : 'إضافة'}}</b-button>
            <b-button to="/packages" class="ml-1">تراجع</b-button>
        </b-card-footer>
    </b-card>
</template>
<script>
import { ValidationObserver } from "vee-validate";
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";
import EKInputPicker from "@Ekcore/components/EK-forms/EK-input-picker";
import EKInputText from "@Ekcore/components/EK-forms/EK-input-text";
import { mapState, mapGetters, mapActions } from "vuex";
import EKTableCollapse from "@Ekcore/components/EK-table-collapse";
import EKPagination from "@Ekcore/components/EK-pagination";

export default {
    components: {
        ValidationObserver,
        EKInputSelect,
        EKInputPicker,
        EKInputText,
        EKTableCollapse,
        EKPagination
    },
    props: {
        id: String
    },
    data: () => ({
        activeTabsFaculty: [],
        columns: [
            {
                label: "اسم المادة",
                value: "name"
            },
            {
                label: "وصف المادة",
                value: "description"
            },
            {
                label: "تفاصيل",
                value: "details",
                sortable: false
            }
        ],
        examHeader: [
            {
                label: "تابع ل",
                value: "name"
            },
            {
                label: "السنة",
                value: "year"
            },
            {
                label: "النوع",
                value: "type"
            },
            {
                label: "السعر",
                value: "price"
            },
            {
                label: "تفاصيل",
                value: "details",
                sortable: false
            }
        ]
    }),
    computed: {
        ...mapState({
            packageDto: state => state.packages.packageDto,
            tabsFaculty: state => state.packages.tabsFaculty,
            faculties: state => state.faculties.faculties,
            year: state => state.globalStore.year,
            filterDto: state => state.packages.filterDto
        }),
        ...mapGetters(["semester"])
    },
    mounted() {
        this.getFacultiesDetails();
        this.fetchTotalTag();
        if (this.id == (0 | "")) this.localeFetchTabsFacultyBy(this.filterDto);
        else this.packageDetails(this.id);
    },
    methods: {
        ...mapActions([
            "fetchTabsFacultyBy",
            "getFacultiesDetails",
            "fetchTotalTag",
            "addPackage",
            "packageDetails",
            "setPackage"
        ]),
        calcPackagePrice() {
            this.packageDto.price = 0;
            this.tabsFaculty.forEach(sub => {
                sub.exams.forEach(exam => {
                    if (this.packageDto.selectedExams.indexOf(exam.id) != -1) {
                        this.packageDto.price += exam.price;
                    }
                });
            });
        },
        onSubmit() {
            this.$refs.observer.validate().then(async suc => {
                if (suc && this.packageDto.selectedExams.length) {
                    let exams = [];
                    await this.packageDto.selectedExams.forEach(id => {
                        this.tabsFaculty.forEach(sub => {
                            let item = sub.exams.find(fac => fac.id == id);
                            if (item) {
                                exams.push({
                                    price: item.price,
                                    examId: item.id
                                });
                            }
                        });
                    });
                    if (this.id != (0 & "")) {
                        this.setPackage({
                            id: this.packageDto.id,
                            name: this.packageDto.name,
                            description: this.packageDto.description,
                            price: this.packageDto.price,
                            startDate: new Date(this.packageDto.startDate),
                            endDate: new Date(this.packageDto.endDate),
                            type: +this.packageDto.type,
                            isHidden: this.packageDto.isHidden,
                            exams
                        });
                    } else {
                        this.addPackage({
                            name: this.packageDto.name,
                            description: this.packageDto.description,
                            price: this.packageDto.price,
                            startDate: new Date(this.packageDto.startDate),
                            endDate: new Date(this.packageDto.endDate),
                            type: +this.packageDto.type,
                            isHidden: this.packageDto.isHidden,
                            exams
                        });
                    }
                }
            });
        },
        localeFetchTabsFacultyBy(filterDto) {
            this.$refs.filterTabsFaculity.validate().then(success => {
                if (success) {
                    this.packageDto.selectedExams = [];
                    this.fetchTabsFacultyBy(filterDto);
                }
            });
        },
        chaingeSelect(props) {
            this.packageDto.selectedExams = [...props];
        },
        changeSelectChildren(props) {
            console.log(props)
            this.packageDto.selectedExams = [...props];
            this.calcPackagePrice();
        },
        details(dat) {
            console.log(dat);
        }
    },
    beforeDestroy() {
        this.$store.commit("Reset_Package_Dto");
    }
};
</script>
