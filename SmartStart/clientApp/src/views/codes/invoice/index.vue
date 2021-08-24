  <template>
    <EKTable
        :items="usersInvoicesList"
        :columns="columns"
        selectedLabel="name"
        @details="openSubjectDEtails"
        no_select
    >
        <template slot="items.lastMatchDate" slot-scope="{ value }">
            {{(new Date("0001-01-01T00:00:00").getTime() == new Date(value).getTime() || !value) ? '---' : new Date(value).toLocaleDateString("en-GB")}}
        </template>
    </EKTable>
</template>
<script>
import EKTable from "@Ekcore/components/EK-table";
import {  mapActions, mapGetters } from 'vuex';
export default {
    components: {
        EKTable
    },
    data: () => ({
        columns: [
            {
                label: "اسم نقطة البيع",
                field: "posName"
            },
            {
                label: "عدد الاكواد",
                field: "codeCount",
                type: "number" 
            },
            {
                label: "القيمة المدفوعة",
                field: "paidMoney",
                type: "number" 
            },
            {
                label: "القيمة المستحقة",
                field: "balanceDue",
                type: "number"
            },
            {
                label: "تاريخ اخر دفعة",
                field: "lastMatchDate"
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
            "usersInvoicesList"])
    },
    created() {
        this.getUsersInvoice()
    },
    methods: {
        ...mapActions(["getUsersInvoice"]),
        openSubjectDEtails(props) {
            this.$router.push("/invoice/" + props.row.posId);
        }
    },
     beforeDestroy() {
        this.$store.commit('Reset_Search_Dto')
    },
};
</script>
