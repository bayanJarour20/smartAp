<template>
    <div class="p-5 text-right">
        <h1>EK table collapse</h1>
        <EKTableCollapse
            label="label"
            selectLabel="selected"
            :rows="rows"
            :columns="cols"
            childId="id"
            childrenLabel="children"
            customHeaderLabel="label"
            :colapseHeader="[]"
        >
        </EKTableCollapse>
        <hr>
        <h2>Documentation</h2>
        <h4># Props</h4>
        <ul class="list-unstyled p-0 m-0">
            <li>1_ label: String -- default: () => "label" </li>
            <li>2_ selectLabel: String -- default: () => "selected" </li>
            <li>3_ childId: String -- default: () => "id" </li>
            <li>4_ rows: Array -- default: () => "[]" </li>
            <li>5_ columns: Array -- default: () => "[]" </li>
            <li>6_ childrenLabel: String -- default: () => "children" </li>
            <li>7_ customHeaderLabel: String -- default: () => "label" </li>
            <li>8_ colapseHeader: Array -- default: () => "[]" </li>
        </ul>
        <hr>
        <h4># Events</h4>
        <ul class="list-unstyled p-0 m-0">
            <li>1_ delete-selected event emit on header trash button click</li>
            <li>2_ details event emit on default details button click</li>
            <li>3_ changeParentcheck event emit on checkbox change</li>
            <li>4_ changeSelectChildren event emit on children checkbox change</li>
        </ul>
        <hr>
        <h4># Slots</h4>
        <ul class="list-unstyled p-0 m-0">
            <li>1_ { header } parent table header thead content </li>
            <small class="pr-2">scope-slot: headers</small>
            <li>2_ { header.value } parent table header items th content </li>
            <small class="pr-2">scope-slot: th</small>
            <li>3_ { body } parent table body tbody content </li>
            <small class="pr-2">scope-slot: rows</small>
            <li>4_ { item-value } parent table body items td content </li>
            <small class="pr-2">scope-slot: tr -- val</small>
            <li>5_ { sub-select-row } sub table checkbox slot </li>
            <small class="pr-2">scope-slot: tr -- val</small>
            <li>6_ { item-td } sub table each td content </li>
            <small class="pr-2">scope-slot: tr</small>
            <li>7_ { item-td.value } sub table td content </li>
            <small class="pr-2">scope-slot: tr -- td</small>
        </ul>
    </div>
</template>
<style lang="scss" scoped>
ul li, h4 {
    direction: rtl
}
</style>
<script>
import EKTableCollapse from "@Ekcore/components/EK-table-collapse";
export default {
    components: {
        EKTableCollapse
    },
    data: () => ({
        cols: [{label: 'idas', value: 'id'}, {label: 'name', value: 'name'}, {label: 'password', value: 'password'}],
        rows: [{id: 1, name: 'name', password: '323s3', children: [{id: 1, name: 'name', password: '11111'}]},
        {id: 1, name: 'name', password: '323s3', children: [{id: 1, name: 'name', password: '11111'}]},
        {id: 1, name: 'name', password: '323s3', children: [{id: 1, name: 'name', password: '11111'}]},
        {id: 1, name: 'name', password: '323s3', children: [{id: 1, name: 'name', password: '11111'}]}]
    })
}
</script>