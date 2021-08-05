<template>
    <b-card no-body class="mb-1">
        <b-card-header class="py-0">
            <b-button
                size="sm"
                variant="flat-secondary"
                class="btn-icon rounded-circle ml-auto"
                style="padding: 0.5rem 10px !important"
                @click="getSelectedRows()"
                :disabled="isNotSelectedRows"
                v-if="!no_delete"
            >
                <unicon name="trash-alt" width="18"></unicon>
            </b-button>
        </b-card-header>
        <b-card-body class="p-0">
            <EKTableCollapse
                v-if="true"
                @selectChange="selectionChanged"
                @changeSelectChildren="changeSelectChildren"
                :label="label"
                align="center"
                :headers="columns"
                :items="rows"
                :innerValue="innerValue"
                :colapseOptions="{
                    enable: true,
                    childrenLabel,
                    enableCustomHeadre: true,
                    customHeaderLabel,
                    header: colapseHeader
                }"
                :selectOptions="{
                    enable: true,
                    label: selectLabel
                }"
                :lableId="childId"
                borderd
            >
                <template slot="selectAll">
                    <span></span>
                </template>
                <!-- // icon -->
                <template slot="colapse-icon">
                    <unicon name="angle-up" width="22" height="22" />
                </template>
                <!-- // header -->
                <template slot="header" slot-scope="{ headers }">
                    <slot name="header" :headers="headers"></slot>
                </template>
                <template
                    v-for="col in columns"
                    :slot="'header.' + col.value"
                    slot-scope="{ th }"
                >
                    <slot :name="'header.' + col.value" :th="th"></slot>
                </template>
                <!-- // body -->
                <template slot="body" slot-scope="{ rows }">
                    <slot name="body" :rows="rows"></slot>
                </template>
                <template
                    v-for="th in columns"
                    :slot="['item-' + th.value]"
                    slot-scope="{tr}"
                >
                    <slot
                        :name="['item-' + th.value]"
                        :tr="tr"
                        :val="tr[th.value]"
                    ></slot>
                </template>
                <template
                    slot="item-details"
                    slot-scope="{ tr }"
                >
                    <b-button
                        size="sm"
                        variant="flat-secondary"
                        class="btn-icon rounded-circle"
                        style="padding: 2px 6px !important"
                        @click="details(tr)"
                    >
                        <unicon name="ellipsis-v" width="18"></unicon>
                    </b-button>
                </template>
                <!-- // children -->
                <!-- // children checkbox -->
                <template slot="sub-select-row" slot-scope="{ tr, val }">
                    <slot name="sub-select-row" :tr="tr" :val="val"></slot>
                </template>
                <!-- // children body item out of td -->
                <template
                    v-for="th in !!colapseHeader && colapseHeader.length
                        ? colapseHeader
                        : columns"
                    :slot="'item-th.' + th.value"
                    slot-scope="{ tr, parent, trIndex }"
                >
                    <slot :name="'item-td.' + th.value" :tr="tr" :trIndex="trIndex" :parent='parent'></slot>
                </template>
                
                <template
                    slot="item-th.details"
                    slot-scope="{ tr }"
                >
                    <td>
                        <b-button
                            size="sm"
                            variant="flat-secondary"
                            class="btn-icon rounded-circle"
                            style="padding: 2px 6px !important"
                            @click="details(tr)"
                        >
                            <unicon name="ellipsis-v" width="18"></unicon>
                        </b-button>
                    </td>
                </template>
                <!-- // children body item inside of td-->
                <template slot="item-th" slot-scope="{ tr, th }">
                    <slot name="item-td" :tr="tr" :th="th"></slot>
                </template>
            </EKTableCollapse>
        </b-card-body>
        <slot name="table-footer">
            <b-card-footer class="p-1 border-0">
            </b-card-footer>
        </slot>
    </b-card>
</template>
<script>
import EKTableCollapse from "./EK-table-collapse";
import { BCard, BCardHeader, BCardBody, BButton } from "bootstrap-vue";
export default {
    components: {
        EKTableCollapse,
        // bootstrap vue
        BCard,
        BCardHeader,
        BCardBody,
        BButton
    },
    props: {
        label: {
            type: String,
            default: () => "label"
        },
        selectLabel: {
            type: String,
            default: () => "selected"
        },
        childId: {
            type: String,
            default: () => "id"
        },
        rows: {
            type: Array,
            default: () => []
        },
        columns: {
            type: Array,
            default: () => []
        },
        childrenLabel: {
            type: String,
            default: () => "children"
        },
        customHeaderLabel: {
            type: String,
            default: () => "label"
        },
        colapseHeader: {
            type: Array,
            default: () => []
        },
        no_delete: Boolean,
        innerValue: {
            type: Array,
            default: () => []
        }
    },
    data: () => ({
        selectedRow: [],
        selectedChildRow: [],
        isNotSelectedRows: false
    }),
    methods: {
        async selectionChanged(id) {
            if (await this.selectedRow.indexOf(id) == -1) {
                this.selectedRow.push(id);
            } else {
                const index = await this.selectedRow.findIndex(
                    selectedId => selectedId == id
                );
                this.selectedRow.splice(index, 1);
            }
            this.$emit('changeParentcheck', this.selectedRow)
        },
        async changeSelectChildren(payload) {
            if(!payload.type) {
                if (this.selectedChildRow.indexOf(payload.tr[this.childId]) == -1) {
                    this.selectedChildRow.push(payload.tr[this.childId]);
                } else {
                    const index = this.selectedChildRow.findIndex(
                        selectedId => selectedId == payload.tr[this.childId]
                    );
                    this.selectedChildRow.splice(index, 1);
                }
            } else {
                if(payload.tr.selected) {
                    if (this.selectedChildRow.indexOf(payload.tr[this.childId]) == -1) {
                        this.selectedChildRow.push(payload.tr[this.childId]);
                    }
                } else {
                    let index = this.selectedChildRow.indexOf(payload.tr[this.childId])
                    if (index != -1) {
                        this.selectedChildRow.splice(index, 1);
                    }
                }
            }
            if(payload.last) {
                this.$emit('changeSelectChildren', this.selectedChildRow)
            }
        },
        getSelectedRows() {
            this.selectedIds = [];
            this.$emit("delete-selected", this.selectedIds);
        },
        details(props) {
            this.$emit("details", props);
        }
    }
};
</script>
