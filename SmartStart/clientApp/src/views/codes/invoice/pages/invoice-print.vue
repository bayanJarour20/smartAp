<template>
    <div>
        <b-row>
            <b-col cols="12">
                <b-card no-body>
                    <b-card-header class="p-0 position-fixed flex-column">
                        <div class="d-flex w-100 justify-content-between">
                            <div>
                                <div class="d-flex mb-2 align-items-center">
                                    <h3 class="text-primary pr-2 pl-2">
                                        <strong>Tarafoua</strong>
                                    </h3>
                                    <img
                                        src="/favicon.png"
                                        width="32"
                                        height="32"
                                        alt=""
                                    />
                                </div>
                                <p style="width: 300px" class="text-right">
                                    Lorem ipsum dolor sit amet consectetur,
                                    adipisicing elit. Maiores incidunt, soluta
                                    iure obcaecati dolore ipsum nobis commodi
                                    porro
                                </p>
                            </div>
                            <div>
                                <b-form-group
                                    class="align-items-center font-weight-bolder"
                                    label="فاتورة رقم :"
                                    label-cols="4"
                                    label-size="sm"
                                    label-for="invoceNumber"
                                >
                                    <b-form-input
                                        style="width:120px"
                                        class="ml-auto bg-white border-0"
                                        id="invoceNumber"
                                        type="text"
                                        readonly
                                        size="sm"
                                        v-model="invoiceDto.invoiceNumber"
                                    >
                                    </b-form-input>
                                </b-form-group>
                                <b-form-group
                                    class="align-items-center font-weight-bolder"
                                    label="تاريخ الفاتورة :"
                                    label-cols="4"
                                    label-size="sm"
                                    label-for="invoceDate"
                                >
                                    <b-form-input
                                        style="width:120px"
                                        class="ml-auto bg-white border-0"
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
                        </div>
                        <b-col cols="12">
                            <hr />
                        </b-col>
                    </b-card-header>
                    <table>
                        <thead>
                            <tr>
                                <td>
                                    <div class="page-header-space"></div>
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    <b-card-body class="p-0">
                                        <vue-good-table
                                            :columns="columns"
                                            :rows="invoiceDto.codes"
                                            :rtl="false"
                                            :small="true"
                                            styleClass="vgt-table condensed border-0"
                                            ref="accounts-table"
                                        >
                                            <template
                                                slot="table-row"
                                                slot-scope="props"
                                            >
                                                <span
                                                    v-if="props.column.field === 'date'"
                                                >
                                                    {{
                                                        new Date(
                                                            props.formattedRow[
                                                                props.column.field
                                                            ]
                                                        ).toISOString()
                                                            .substr(0, 10)
                                                    }}
                                                </span>
                                                <span
                                                    v-else-if="props.column.field === 'discountRate'"
                                                >
                                                    {{
                                                            props.formattedRow[
                                                                props.column.field
                                                            ]
                                                    }} %
                                                </span>
                                                <span v-else>
                                                    {{
                                                        props.formattedRow[
                                                            props.column.field
                                                        ]
                                                    }}
                                                </span>
                                            </template>
                                        </vue-good-table>
                                        <b-row class="pt-1">
                                            <b-col cols="8" class="pt-2">
                                                <strong
                                                    >مسؤول المبيعات :
                                                </strong>
                                                {{ invoiceDto.posName }}
                                            </b-col>
                                            <b-col cols="4">
                                                <b-row>
                                                    <b-col cols="12">
                                                        <ul
                                                            class="list-unstyled p-0 m-0"
                                                        >
                                                            <li
                                                                class="d-flex pb-1 pt-2 justify-content-between align-items-center"
                                                            >
                                                                <div>
                                                                    عدد الأكواد
                                                                </div>
                                                                <div>
                                                                    {{
                                                                        invoiceDto.codeCount
                                                                    }}
                                                                    ل.س
                                                                </div>
                                                            </li>
                                                            <li
                                                                class="d-flex pt-1 justify-content-between align-items-center"
                                                            >
                                                                <div>
                                                                    الكلفة
                                                                </div>
                                                                <div>
                                                                    {{
                                                                        invoiceDto.actualCost
                                                                    }}
                                                                    ل.س
                                                                </div>
                                                            </li>
                                                        </ul>
                                                    </b-col>
                                                    <b-col cols="12">
                                                        <hr />
                                                    </b-col>
                                                    <b-col cols="12">
                                                        <ul
                                                            class="list-unstyled p-0"
                                                        >
                                                            <li
                                                                class="d-flex justify-content-between align-items-center"
                                                            >
                                                                <div>
                                                                    المستحق لنا
                                                                </div>
                                                                <div>
                                                                    {{
                                                                        invoiceDto.dueCompany
                                                                    }}
                                                                    ل.س
                                                                </div>
                                                            </li>
                                                        </ul>
                                                        <div>
                                                            <b-input-group>
                                                                <b-input-group-prepend
                                                                    is-text
                                                                    class="pr-1"
                                                                >
                                                                    المبلغ
                                                                    المدفوع
                                                                </b-input-group-prepend>
                                                                <b-form-input
                                                                    type="number"
                                                                    class="border-0 text-right"
                                                                    style="padding-left: 4px!important"
                                                                    v-model="
                                                                        invoiceDto.paidMoney
                                                                    "
                                                                ></b-form-input>
                                                                <b-input-group-append
                                                                    is-text
                                                                >
                                                                    ل.س
                                                                </b-input-group-append>
                                                            </b-input-group>
                                                        </div>
                                                    </b-col>
                                                </b-row>
                                            </b-col>
                                            <b-col cols="12">
                                                <hr />
                                            </b-col>
                                            <b-col cols="12">
                                                <p>
                                                    <strong>ملاحظة : </strong>
                                                    {{ invoiceDto.notes }}
                                                </p>
                                            </b-col>
                                        </b-row>
                                    </b-card-body>
                                </td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr>
                                <td>
                                    <div class="page-footer-space"></div>
                                </td>
                            </tr>
                        </tfoot>
                    </table>
                </b-card>
            </b-col>
        </b-row>
    </div>
</template>
<script>
import { VueGoodTable } from "vue-good-table";

import { mapActions, mapState } from "vuex";
export default {
    props: {
        invoiceId: String,
        id: String
    },
    components: {
        VueGoodTable
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
                field: "posRate"
            },
            {
                label: "الحسم %",
                field: "discountRate"
            },
            {
                label: "الكلفة",
                field: "value"
            }
        ]
    }),
    created() {
        this.getInvoiceById(this.invoiceId).then(() => {
            setTimeout(() => {
                this.print();
                this.$router.push('/invoice/' + this.id + '/set/' + this.invoiceId)
            }, 1000);
        });
    },
    computed: {
        ...mapState({
            invoiceDto: state => state.invoices.invoiceDto
        })
    },
    methods: {
        ...mapActions(["getInvoiceById"]),
        print() {
            document.title = "فاتورة_" + this.invoiceDto.posName + '_' + new Date().toLocaleDateString('en-GB')
            window.print();
        }
    }
};
</script>

<style lang="scss">
@import "@/assets/scss/components/invoicePrint.scss";
.removed {
    background: #ea545560 !important;
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
