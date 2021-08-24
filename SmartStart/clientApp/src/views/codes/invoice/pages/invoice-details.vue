<template>
    <div>
        <b-row>
            <b-col cols="12" lg="10">
                <b-card no-body>
                    <b-card-header>
                        <div>
                            <div class="d-flex mb-2 align-items-center">
                                <h3 class="text-primary pr-2">
                                    <strong>Smart Start</strong>
                                </h3>
                                <vuexy-logo width="32" height="32" />
                            </div>
                            <p style="width: 300px" class="text-right"></p>
                        </div>
                        <div>
                            <b-form-group
                                class="align-items-center"
                                label="فاتورة رقم"
                                label-cols="4"
                                label-size="sm"
                                label-for="invoceNumber"
                            >
                                <b-form-input
                                    style="width:120px"
                                    class="ml-auto"
                                    id="invoceNumber"
                                    type="text"
                                    readonly
                                    size="sm"
                                    v-model="invoiceDto.invoiceNumber"
                                >
                                </b-form-input>
                            </b-form-group>
                            <b-form-group
                                class="align-items-center"
                                label="تاريخ الفاتورة"
                                label-cols="4"
                                label-size="sm"
                                label-for="invoceDate"
                            >
                                <b-form-input
                                    style="width:120px"
                                    class="ml-auto"
                                    id="invoceDate"
                                    type="text"
                                    readonly
                                    size="sm"
                                    :value="
                                        new Date(invoiceDto.date)
                                            .toISOString()
                                            .substr(0, 10)
                                    "
                                >
                                </b-form-input>
                            </b-form-group>
                        </div>
                    </b-card-header>
                    <b-card-body>
                        <EKTable
                            :columns="columns"
                            :items="invoiceDto.codes"
                            selectedLabel="name"
                            no_delete
                            :no_select="mode"
                            @selected-rows="getSelectedCodes"
                            :row-style-class="isRemoved"
                        >
                            <template slot="items.date" slot-scope="{ value }">
                                {{
                                    new Date(value).toLocaleDateString("en-GB")
                                }}
                            </template>
                        </EKTable>
                        <h6
                            class="text-danger text-center"
                            v-if="!this.selectedCodes.length && !mode"
                        >
                            يجب تحديد كود واحد على الأقل
                        </h6>
                        <b-row class="mt-2">
                            <b-col cols="8">
                                <b-form-group label="ملاحظات" label-for="note">
                                    <b-form-textarea
                                        v-model="invoiceDto.notes"
                                        id="note"
                                        rows="6"
                                    ></b-form-textarea>
                                </b-form-group>
                            </b-col>
                            <b-col cols="4">
                                <b-row>
                                    <b-col cols="6" lg="12">
                                        <ul class="list-unstyled p-0 m-0">
                                            <li
                                                class="d-flex pb-1 pt-2 justify-content-between align-items-center"
                                            >
                                                <div>عدد الأكواد</div>
                                                <div>
                                                    {{ !mode ? selectedCodes.length : invoiceDto.codeCount }}
                                                </div>
                                            </li>
                                            <li
                                                class="d-flex pt-1 justify-content-between align-items-center"
                                            >
                                                <div>الكلفة</div>
                                                <div>{{ !mode ? totalValue : invoiceDto.actualCost }} ل.س</div>
                                            </li>
                                        </ul>
                                    </b-col>
                                    <b-col cols="12">
                                        <hr class="d-none d-lg-block" />
                                    </b-col>
                                    <b-col cols="6" lg="12">
                                        <ul class="list-unstyled p-0">
                                            <li
                                                class="d-flex justify-content-between align-items-center"
                                            >
                                                <div>المستحق لنا</div>
                                                <div>
                                                    {{ !mode ? lastTotalValue : invoiceDto.dueCompany }} ل.س
                                                </div>
                                            </li>
                                        </ul>
                                        <ValidationObserver
                                            ref="observerInvoice"
                                        >
                                            <div>
                                                <EKInputText
                                                    v-if="!mode"
                                                    v-model="
                                                        invoiceDto.paidMoney
                                                    "
                                                    label="المبلغ المدفوع"
                                                    :labelCols="6"
                                                    name="paidValue"
                                                    type="number"
                                                    :rules="[
                                                        {
                                                            type: 'required',
                                                            message:
                                                                'المبلغ المدفوع مطلوب'
                                                        },
                                                        {
                                                            type: 'min_value:0',
                                                            message:
                                                                'يجب ان تكون القيمة موجبة'
                                                        }
                                                    ]"
                                                />
                                                <EKInputText
                                                    v-else
                                                    :labelCols="6"
                                                    :value="
                                                        invoiceDto.paidMoney
                                                    "
                                                    label="المبلغ المدفوع"
                                                    name="paidValue"
                                                    type="number"
                                                    readonly
                                                />
                                            </div>
                                        </ValidationObserver>
                                    </b-col>
                                </b-row>
                            </b-col>
                        </b-row>
                    </b-card-body>
                </b-card>
            </b-col>
            <b-col cols="12" lg="2">
                <b-card>
                    <b-button
                        variant="primary"
                        block
                        v-if="!mode"
                        @click="onSubmit(invoiceDto, 0)"
                        >حفظ</b-button
                    >
                    <b-button
                        variant="outline-primary"
                        block
                        v-if="!mode"
                        @click="onSubmit(invoiceDto, 1)"
                        >حفظ و طباعة</b-button
                    >
                    <b-button
                        variant="outline-primary"
                        block
                        v-else
                        @click="print()"
                        >طباعة</b-button
                    >
                    <b-button
                        variant="outline-primary"
                        :to="'/invoice/' + id"
                        block
                        >عودة</b-button
                    >
                </b-card>
            </b-col>
        </b-row>
    </div>
</template>
<script>
import { mapActions, mapState } from "vuex";
import EKInputText from "@Ekcore/components/EK-forms/EK-input-text";
import VuexyLogo from "@core/layouts/components/Logo.vue";
import EKTable from "@Ekcore/components/EK-table";
import { ValidationObserver } from "vee-validate";
export default {
    components: {
        VuexyLogo,
        EKTable,
        EKInputText,
        ValidationObserver
    },
    props: {
        invoiceId: String,
        id: String
    },
    data: () => ({
        columns: [
            {
                label: "",
                field: "remove",
                sortable: false
            },
            {
                label: "الكود",
                field: "code"
            },
            {
                label: "التاريخ",
                field: "date"
            },
            {
                label: "النسبة",
                field: "posRate",
                type: "number"
            },
            {
                label: "الحسم %",
                field: "discountRate",
                type: "number"
            },
            {
                label: "الكلفة",
                field: "value",
                type: "number"
            }
        ],
        mode: false,
        selectedCodes: [],
        totalValue: 0,
        lastTotalValue: 0
    }),
    computed: {
        ...mapState({
            invoiceDto: state => state.invoices.invoiceDto
        })
    },
    created() {
        this.mode = this.invoiceId != ("" | 0);
        if (this.invoiceId != ("" | 0)) {
            this.getInvoiceById(this.invoiceId);
        } else {
            this.fillInvoice(this.id);
        }
    },
    methods: {
        ...mapActions(["getInvoiceById", "fillInvoice", "createInvoice"]),
        getSelectedCodes(ids) {
            this.totalValue = 0;
            this.lastTotalValue = 0;
            this.selectedCodes = ids;
            this.selectedCodes.forEach(code => {
                this.totalValue += code.value;
                this.lastTotalValue +=
                    code.value - (code.value * code.posRate) / 100;
            });
        },
        onSubmit(dto, isPrint) {
            this.$refs.observerInvoice.validate().then(success => {
                if (success && this.selectedCodes.length) {
                    this.createInvoice({dto: {
                        posId: this.id,
                        invoiceNumber: this.invoiceDto.invoiceNumber,
                        date: new Date(this.invoiceDto.date),
                        paidMoney: this.invoiceDto.paidMoney,
                        notes: this.invoiceDto.notes,
                        codes: this.selectedCodes
                    }, isPrint});
                }
            });
        },
        print() {
            this.$router.push(
                "/invoice/" + this.id + "/print/" + this.invoiceId
            );
        },
        isRemoved(row) {
            return !row.vgtSelected && !this.mode ? "removed" : "";
        }
    }
};
</script>

<style lang="scss">
.removed {
    background: #ea545540 !important;
}
.input-group-append {
    .input-group-text {
        border-radius: 0 0.357rem 0.357rem 0 !important;
    }
}
.input-group-prepend {
    .input-group-text {
        border-radius: 0.357rem 0 0 0.357rem !important;
    }
}
</style>
