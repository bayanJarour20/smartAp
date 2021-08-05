<template>
    <div class="vc__table-container" :class="[{ 'borderd': !!borderd }]">
        <table
            class="vc__table"
            :class="[tableClass]"
            border="0"
            cellspacing="0"
            cellpadding="0"
        >
            <thead class="vc__table__thead">
                <slot name="header" :headers="headers">
                    <tr
                        class="vc__table__tr vc__thead__tr"
                        :class="[theadRowClass]"
                        :style="[vc__align]"
                    >
                        <th v-if="selectOptions.enable && rows.length && false" class="selection">
                            <slot name="selectAll">
                                <input type="checkbox" v-model="selectAll">
                            </slot>
                        </th>
                        <th v-for="(th, index) in headers" :key="'th_' + index">
                            <slot :name="'header.' + th.value" :th="th[label]">{{ th[label] }}</slot>
                        </th>
                        <td v-if="colapseOptions.enable && rows.length" class="colapse-icon"></td>
                    </tr>
                </slot>
            </thead>
            <transition name="tab-panel-slide-up">
                <caption v-if="!rows.length">
                    لا يوجد بيانات
                </caption>
                <tbody v-else class="vc__table__tbody">
                    <slot name="body" :rows="rows">
                        <template v-for="(row, index) in rows">
                            <tr
                                class="vc__table__tr vc__tbody__tr"
                                :class="[theadRowClass]"
                                :style="[vc__align]"
                                :key="'tr_' + index"
                                :ref="'tr-' + index"
                            >
                                <td v-if="selectOptions.enable && false" class="selection">
                                    <input type="checkbox" :checked="row[selectOptions.label]" @change="selectChange(row)">
                                </td>
                                <td 
                                    @click="toggleChildren(index)"
                                    v-for="(th, i) in headers"
                                    :key="'td_' + i"
                                >
                                    <slot :name="['item-' + th.value]" :tr="row">
                                        {{ row[th.value] }}
                                    </slot>
                                </td>
                                <td
                                    @click="toggleChildren(index)"
                                    v-if="colapseOptions.enable"
                                    class="colapse-icon"
                                >
                                    <span class="icon" :class="{'colapsed': row.isOpen}">
                                        <slot name="colapse-icon">
                                            ^
                                        </slot>
                                    </span>
                                </td>
                            </tr>
                            <ek-table-colabse-child
                                v-if="colapseOptions.enable"
                                :key="'sub_table_' + index"
                                :label="!colapseOptions.customHeaderLabel ? label : colapseOptions.customHeaderLabel"
                                :open="row.isOpen"
                                :selectOptions="selectOptions"
                                :mainTableLength="rows.length"
                                :align="vc__align"
                                :childrenLabel="childrenLabel"
                                @selectChange="select"
                                :isHeader="colapseOptions.enableCustomHeadre"
                                :headers="
                                    !!(
                                        !!colapseOptions &&
                                        !!colapseOptions.header &&
                                        colapseOptions.header.length
                                    )
                                        ? colapseOptions.header
                                        : headers
                                "
                                :item="row"
                                :borderd="!!borderd"
                            >
                                <template slot="sub-select-row" slot-scope="{ tr, val }">
                                    <slot name="sub-select-row" :tr="tr" :val="val"></slot>
                                </template>
                                <template v-for="th in !!(
                                        !!colapseOptions &&
                                        !!colapseOptions.header &&
                                        colapseOptions.header.length
                                    )
                                        ? colapseOptions.header
                                        : headers" :slot="'item-th.' + th.value" slot-scope="{tr, trIndex}">
                                    <slot :name="'item-th.' + th.value" :tr="tr" :trIndex="trIndex" :parent="row"></slot>
                                </template>
                                <template slot="item-th" slot-scope="{tr, th}">
                                    <slot name="item-th" :tr="tr" :th="th"></slot>
                                </template>
                            </ek-table-colabse-child>
                        </template>
                    </slot>
                </tbody>
            </transition>
            <slot name="footer"></slot>
        </table>
    </div>
</template>
<script>
import ekTableColabseChild from "./EK-table-colabse-child.vue";

export default {
    components: {
        ekTableColabseChild
    },
    data: () => ({
        selectAll: false,
        value: []
    }),
    props: {
        // options
        colapseOptions: Object,
        selectOptions: Object,
        // style
        align: String,
        // table header row
        headers: {
            type: Array,
            required: true
        },
        label: {
            type: String,
            required: true
        },
        // table body rows
        items: {
            type: Array,
            required: true
        },
        lableId: String,
        innerValue: {
            type: Array,
            default: function() {
                return []
            }
        },
        // custom classes
        tableClass: Array,
        theadRowClass: Array,
        tbodyRowClass: Array,
        borderd: Boolean
    },
    computed: {
        rows: function() {
            return this.items.filter(item => {
                item = Object.assign({}, item, { isOpen: false });
                return item;
            });
        },
        vc__align: function() {
            return !this.align ? {} : { "text-align": this.align };
        },
        selectedLabel: function() {
            return !this.selectOptions.label ? 'selected' : this.selectOptions.label
        },
        childrenLabel: function() {
            return !this.colapseOptions.childrenLabel ? 'children' : this.colapseOptions.childrenLabel
        },
        id: function() {
            return !this.lableId ? 'id' : this.lableId
        }
    },
    methods: {
        toggleChildren(index) {
            this.$set(this.rows[index], "isOpen", !this.rows[index].isOpen);
        },
        selectChange(tr) {
            this.$emit('selectChange', !this.lableId ? tr : tr[this.id])
            const selectValue = !tr[this.selectedLabel]
            this.$set(tr, this.selectedLabel, selectValue)
            tr[this.childrenLabel].forEach(child => {
                this.$set(child, this.selectedLabel, selectValue)
            });
            this.select(tr, 1)
        },
        select(obj, type) {
            console.log(type, '---' , obj, obj[this.selectedLabel])
            if(type && obj[this.selectedLabel]) {
                obj[this.childrenLabel].forEach((child, index, list) => {
                    if(child[this.selectedLabel]) {
                        if(index == list.length - 1) {
                            this.$emit('changeSelectChildren', {tr: child, type, last: true})
                        } else {
                            this.$emit('changeSelectChildren', {tr: child, type, last: false})
                        }
                        this.value.unshift(child)
                    }
                })
            } else if (type) {
                obj[this.childrenLabel].forEach((child) => {
                    this.value.forEach((item, index, list) => {
                        if(item === child[this.id]) {
                            if(index == list.length - 1) {
                                this.$emit('changeSelectChildren', {tr: child, type, last: true})
                            } else {
                                this.$emit('changeSelectChildren', {tr: child, type, last: false})
                            }
                            list.splice(index, 1)
                        }
                    })
                })
            } else if(!type && obj[this.selectedLabel]) {
                this.$emit('changeSelectChildren', {tr: obj, type, last: true})
                this.value.unshift(obj)
            } else {
                console.log(this.value)
                this.value.map((item, index, list) => {
                    console.log(item)
                    if(item === obj[this.id]) {
                        this.$emit('changeSelectChildren', {tr: obj, type, last: true})
                        list.splice(index, 1)
                    }
                })
            }
        },
    },
    watch: {
        selectAll(val) {
            this.rows.forEach((row) => {
                this.$set(row, this.selectedLabel, val);
                row[this.childrenLabel].forEach((child) => {
                    this.$set(child, this.selectedLabel, val);
                })
            })
        },
        innerValue(val) {
            this.value = val
            this.rows.forEach((row) => {
                row[this.childrenLabel].forEach((child) => {
                    if(this.value.indexOf(child.id) != -1) this.$set(child, this.selectedLabel, true);
                })
            })
        },
        rows() {
            this.rows.forEach((row) => {
                row[this.childrenLabel].forEach((child) => {
                    if(this.value.indexOf(child.id) != -1) this.$set(child, this.selectedLabel, true);
                })
            })
        }
    }
};
</script>
