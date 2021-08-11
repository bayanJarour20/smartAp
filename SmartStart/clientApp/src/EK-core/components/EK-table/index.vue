<template>
    <div>
        <b-card no-body class="mb-1">
            <b-card-header class="py-0">
                <b-button
                    size="sm"
                    variant="flat-secondary"
                    class="btn-icon rounded-circle"
                    style="padding: 0.5rem 10px !important"
                    @click="$emit('plus')"
                    v-if="isPlus"
                >
                    <unicon name="plus" width="18"></unicon>
                </b-button>
                <h5 class="ml-auto mb-0" v-if="title">{{title}}</h5>
                <b-button
                    size="sm"
                    variant="flat-secondary"
                    class="btn-icon rounded-circle ml-auto"
                    style="padding: 0.5rem 10px !important"
                    @click="getSelectedRows()"
                    v-if="!no_select && !no_delete"
                    :disabled="isNotSelectedRows"
                >
                    <unicon name="trash-alt" width="18"></unicon>
                </b-button>
            </b-card-header>
            <b-card-body class="p-0">
                <vue-good-table
                    v-if="list.length > 0"
                    @on-selected-rows-change="selectionChanged"
                    :columns="columns"
                    :rows="activeItemsList"
                    :rtl="false"
                    v-bind="$attrs"
                    :small="true"
                    styleClass="vgt-table condensed"
                    ref="table"
                    @on-sort-change="onSortChange"
                    :select-options="{
                        enabled: !no_select,
                        selectOnCheckboxOnly: true, // only select when checkbox is clicked instead of the row
                        disableSelectInfo: true, // disable the select info panel on top
                        selectAllByGroup: true // when used in combination with a grouped table, add a checkbox in the header row to check/uncheck the entire group
                    }"
                >
                    <template slot="table-column" slot-scope="props">
                        <slot :name="'headers.' + props.column.field" :props="props">
                            {{props.column.label}}
                        </slot>
                    </template>
                    <template slot="table-row" slot-scope="props">
                        <span v-if="props.column.field === 'details'">
                            <slot name="items.details" :props="props">
                                <b-button
                                    size="sm"
                                    variant="flat-secondary"
                                    class="btn-icon rounded-circle"
                                    style="padding: 2px 6px !important"
                                    @click="details(props)"
                                >
                                    <unicon name="ellipsis-v" width="18"></unicon>
                                </b-button>
                            </slot>
                        </span>
                        <span v-else>
                            <slot :name="'items.' + props.column.field" :value="props.formattedRow[props.column.field]" :props="props">
                                {{ props.formattedRow[props.column.field] }}
                            </slot>
                        </span>
                    </template>
                </vue-good-table>
                <div v-else class="text-center justify-content-center mt-2">
                    <h4>لا يوجد بيانات متاحة</h4>
                </div>
            </b-card-body>
            <b-card-footer class="p-1 border-top-0"></b-card-footer>
        </b-card>
        <b-col cols="12" v-if="list.length > 0" class="d-flex justify-content-center mb-3">
            <EKPagination :items="list" v-model="activeItemsList" />
        </b-col>
    </div>
    
</template>
<script>
import { VueGoodTable } from "vue-good-table";
import EKPagination from "@Ekcore/components/EK-pagination";
import {
    BCol,
    BCard,
    BCardHeader,
    BCardBody,
    BButton
} from "bootstrap-vue";
import { sortListObjByArgName } from "@Ekcore/util"

export default {
    components: {
        VueGoodTable,
        EKPagination,
        // bootstrap vue
        BCol,
        BCard,
        BCardHeader,
        BCardBody,
        BButton
    },
    props: {
        items: {
            type: Array,
            required: true
        },
        columns: {
            type: Array,
            required: true
        },
        selectedLabel: {
            type: String,
            default: () => 'id'
        },
        no_delete: {
            type: Boolean,
            default: () => false
        },
        no_select: {
            type: Boolean,
            default: () => false
        },
        isPlus: {
            type: Boolean,
            dafault: () => false
        },
        title: String
    },
    data: () => ({
        activeItemsList: [],
        selectedIds: [],
        isNotSelectedRows: true,
        list: []
    }),
    methods: {
        selectionChanged() {
            this.isNotSelectedRows = !this.$refs["table"].selectedRows
                .length;

            this.$emit("selected-rows", this.$refs["table"].selectedRows);
        },
        getSelectedRows() {
            this.selectedIds = [];
            this.$refs["table"].selectedRows.forEach(row => {
                this.selectedIds.push(row[this.selectedLabel]);
            });
            this.selectionChanged()
            this.$emit("delete-selected", this.selectedIds);
        },
        details(props) {
            this.$emit("details", props);
        },
        onSortChange(params) {
            this.list = sortListObjByArgName(params[0].field, params[0].type, this.items)
        }
    },
    watch: {
        items(l) {
            console.log(l)
            this.list = l
        }
    }
};
</script>
