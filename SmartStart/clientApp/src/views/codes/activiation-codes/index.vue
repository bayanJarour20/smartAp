<template>
    <div>
        <b-button-group class="m-1 mb-2">
            <b-button to="/codes" :variant="+type ? 'outline-primary' : 'primary'">رموز التفعيل</b-button>
            <b-button to="/packages" :variant="+type ? 'primary' : 'outline-primary'">الحزم المتوفرة</b-button>
        </b-button-group>
        <EKTable
            :columns="columns[+type]"
            :items="+type ? packagesList : codesList"
            selectedLabel="name"
            @details="openDetailsActiviationCodes"
            @delete-selected="fireDeleteEvent"
            :row-style-class="rowStyleClassFn"
        >
            <template slot="items.count" slot-scope="{ value }">
                {{value ? value : '---' }}
            </template>
            <template slot="items.dateActivated" slot-scope="{ value }">
                <b-badge :variant="value ? 'success' : 'warning'">{{
                    value ? " مفعل" : " غير مفعل"
                }}</b-badge>
            </template>
            <template slot="items.maxEndDate" slot-scope="{ value }">
                {{new Date("0001-01-01T00:00:00").getTime() == new Date(value).getTime() ? '---' : new Date(value).toLocaleDateString("en-GB")}}
            </template>
            <template slot="items.startDate" slot-scope="{ value }">
                {{ new Date(value).toLocaleDateString("en-GB") }}
            </template>
              <template slot="items.dateCreated" slot-scope="{ value }">
               {{ moment(new Date(value)).format("YYYY/MM/DD HH:mm:ss") }}
            </template>
                <template slot="items.createDate" slot-scope="{ value }">
               {{ moment(new Date(value)).format("YYYY/MM/DD HH:mm:ss") }}
            </template>
            <template slot="items.isHidden" slot-scope="{ value }">
                 <b-badge pill :variant="value ? 'warning' : 'success'">{{value ? 'مخفي' : 'مفعل'}}</b-badge>
            </template>
        </EKTable>
    </div>
</template>
<script>
import EKTable from "@Ekcore/components/EK-table";
import { mapActions, mapGetters } from "vuex";
import moment from "moment";
export default {
    components: {
        EKTable
    },
    data: () => ({
        columns: [
            [
                {
                    label: "الرمز",
                    field: "hash"
                },
                {
                    label: "اسم الطالب",
                    field: "userName"
                },
               {
                    label: "تاريخ الإنشاء",
                    field: "dateCreated",
                    sortable: false
                },
                {
                    label: "القيمة المدفوعة",
                    field: "paidValue",
                    type: "number" 
                },
                {
                    label: "حالة الإشتراك",
                    field: "dateActivated",
                    sortable: false
                },
                {
                    label: "اسم المندوب",
                    field: "sellerName"
                },
                    {
                    label: "تاريخ نهاية الإشتراك",
                    field: "maxEndDate",
                    sortable: false
                },
                {
                    label: "اسم الحزمة",
                    field: "packageName"
                }
            ],
            [
                {
                    label: "اسم الحزمة",
                    field: "name"
                },
                {
                    label: "عدد الرموز",
                    field: "codeCount",
    type: "number" 
                },
                {
                    label: "تاريخ الإنشاء",
                    field: "createDate"
                },
                {
                    label: "كلفة الحزمة",
                    field: "price",
                    type: "number" 
                },
                {
                    label: "حالة الحزمة",
                    field: "isHidden",
                    sortable: false
                },
                {
                    label: "تفاصيل",
                    field: "details",
                    sortable: false
                }
            ]
        ]
    }),
    computed: {
        type() {
            return +!(this.$route.name == 'codes')
        },
        ...mapGetters(['codesList', "packagesList"])
    },
    created() {
        if(+this.type) {
            this.getAllPackages()
        } else {
            this.getAllCodes()
        }
    },
    beforeDestroy() {
        this.$store.commit('Reset_Search_Dto')
    },
    methods: {moment,
        rowStyleClassFn(row) {
            return +this.type && row.type ? 'bg-warning' : '';
        },
        openDetailsActiviationCodes(props) {
            this.$router.push("/packages/set/" + props.row.id);
        },
        fireDeleteEvent(list) {
            console.log(list);
        },
        ...mapActions(["getAllCodes", "getAllPackages"])
    },
    watch: {
        type(type) {
            if(+type) {
                this.getAllPackages()
            } else {
                this.getAllCodes()
            }
        }
    }
};
</script>
