<template>
    <div>
        <EKTable
            :items="invoicesList"
            :columns="columns"
            selectedLabel="name"
            @details="openDetailsActiviationCodes"
            no_select
        >
            <template slot="items.date" slot-scope="{ value }">
                {{new Date(value).toLocaleDateString("en-GB")}}
            </template>
        </EKTable>
    </div>
</template>
<script>
import EKTable from "@Ekcore/components/EK-table";
import {  mapActions, mapGetters } from "vuex";
export default {
    components: {
        EKTable
    },
    props: {
        id: String
    },
    data: () => ({
        columns: [
            {
                label: "رقم الفاتورة",
                field: "invoiceNumber"
            },
            {
                label: "عدد الأكواد",
                field: "codeCount",
                sortable: false
            },
            {
                label: "الكلفة الكلية",
                field: "total"
            },
            {
                label: "تاريخ الإنشاء",
                field: "date"
            },
            {
                label: "تفاصيل",
                field: "details",
                sortable: false
            }
        ]
    }),
    computed: {
        ...mapGetters([
            "invoicesList"
        ])
    },
    created() {
        this.getUserInvoicesById(this.id);
    },
    methods: {
        ...mapActions(["getUserInvoicesById"]),
        openDetailsActiviationCodes(props) {
            this.$router.push(
                "/invoice/" + this.id + "/set/" + props.row.invoiceId
            );
        },
        fireDeleteEvent(list) {
            console.log(list);
        }
    },
      beforeDestroy() {
        this.$store.commit('Reset_Search_Dto')
    },
};
</script>
