<template>
    <div>
        <b-card v-if="type == 0" no-body class="mb-1">
            <b-card-header class="pb-0">
                <strong>فلترة حسب الكلية</strong>
                <b-button
                    type="submit"
                    @click="$store.commit('Set_filter_Dto', filterDto)"
                    variant="primary"
                    >تم</b-button
                >
            </b-card-header>
            <b-card-body class="pb-50">
                <b-row align-v="center">
                    <b-col cols="12" md="4">
                        <EKInputSelect
                            placeholder="اختر الكلية "
                            :options="[{id: 0, name: 'الكل'}, ...faculties]"
                            name="select"
                            :clearable="true"
                            v-model="filterDto.facultyId"
                        />
                    </b-col>
                </b-row>
            </b-card-body>
        </b-card>
        <EKTable
            :items="usersList"
            :columns="columns[type]"
            selectedLabel="id"
            @details="openUsersDetails"
            @delete-selected="usersDeleteEvent"
        >
            <template slot="items.dateBlocked" slot-scope="{value}">
                <b-badge :variant="value ? 'danger' : 'success'">{{value ? "غير مفعل" : 'مفعل'}}</b-badge>
            </template>
            <template slot="items.dateCreated" slot-scope="{value}">
                {{ moment(new Date(value)).format("YYYY/MM/DD HH:mm:ss") }}
            </template>
            <template slot="items.subscriptionDate" slot-scope="{value}">
                {{ moment(new Date(value)).format("YYYY/MM/DD HH:mm:ss") }}
            </template>
            <template v-if="type == 0" slot="items.facultiesIds" slot-scope="{value,props}">
                <div :id="'popover-target' + props.row.id">
                {{ value == null || value.length == 0 ? '-' : value.length == 1 ? facultiesMap.get(value[0]) : facultiesMap.get(value[0]) + '...' }} 
                </div>
                <b-popover v-if="value != null && value.length > 0" :target="'popover-target' + props.row.id" triggers="hover" placement="top">
                    {{ formatFaculties(value) }}
                </b-popover>
            </template>
        </EKTable>
    </div>
</template>
<script>
import EKTable from "@Ekcore/components/EK-table";
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";
import { mapGetters, mapActions, mapState } from "vuex";
import moment from "moment";
export default {
    props: {
        lang: String
    },
    components: {
        EKTable,
        EKInputSelect
    },
    computed: {
        ...mapState({
            faculties: state => state.faculties.faculties,
        }),
        ...mapGetters([
            "usersList",
            "facultiesMap"
        ]),
        type() {
            switch (this.$route.name) {
                case "application-users":
                    return 0;
                case "point-sales":
                    return 1;
                case "panel-users":
                    return 2;
                case "master-users":
                    return 3;
            }
            return false;
        }
    },
    data: () => ({
        columns: [
            [
                {
                    label: "اسم الطالب",
                    field: "name"
                },
                {
                    label: "رقم الهاتف",
                    field: "phoneNumber",
                    sortable: false
                },
                  {
                    label: "عدد الإشتراكات",
                    field: "subscriptionCount",
                    type: "number" 
                },
                {
                    label: "تاريخ الإنشاء",
                    field: "dateCreated"
                },
                {
                    label: "حالة الإشتراك",
                    field: "dateBlocked",
                    sortable: false
                },
                {
                    label: "الكليات",
                    field: "facultiesIds",
                    sortable: false
                },
                {
                    label: "تفاصيل",
                    field: "details",
                    sortable: false
                }
            ],
            [
                {
                    label: "اسم المستخدم",
                    field: "userName"
                },
                {
                    label: "العنوان",
                    field: "address"
                },
                {
                    label: "رقم الهاتف",
                    field: "phoneNumber",
                    sortable: false
                },
                {
                    label: "تاريخ الإشتراك",
                    field: "subscriptionDate"
                },
                {
                    label: "عدد الرموز المباعة",
                    field: "codeSoldCount",
                    type: "number" 
                },
                {
                    label: "حالة الإشتراك",
                    field: "dateBlocked",
                    sortable: false
                },
                {
                    label: "تفاصيل",
                    field: "details",
                    sortable: false
                }
            ],
            [
                {
                    label: "اسم المستخدم",
                    field: "userName"
                },
                {
                    label: "رقم الهاتف",
                    field: "phoneNumber",
                    sortable: false
                },
                {
                    label: "تاريخ الإنشاء",
                    field: "dateCreated"
                },
                {
                    label: "البريد الإلكتروني",
                    field: "email",
                    sortable: false
                },
                {
                    label: "حالة الإشتراك",
                    field: "dateBlocked",
                    sortable: false
                },
                {
                    label: "تفاصيل",
                    field: "details",
                    sortable: false
                }
            ]
        ],
        filterDto: {
            facultyId: 0
        }
    }),
    created() {
        this.getUsers(this.type);
        this.getFacultiesDetails();
        console.log(this.facultiesMap)
        
    },
    methods: {
        ...mapActions(["getUsers" , "getFacultiesDetails","deleteUsersList"]),
        moment,
        openUsersDetails(props) {
            this.$router.push(
                "/users/" + this.type + "/details/" + props.row.id);
        },
        usersDeleteEvent(list) {
            console.log(list)
            this.deleteUsersList(list)
        },
        formatFaculties(arr){
            var s = "";
            arr.forEach( (element , i) => {
                s += this.facultiesMap.get(element);
                if(i != arr.length - 1){
                    s+= "، "
                } 
                else{
                    s += "."
                }
            });
            return s;
        }
    },
    watch: {
        type(type) {
            this.$store.commit('Reset_Search_Dto')
            this.getUsers(type);
        }
    },
     beforeDestroy() {
        this.$store.commit('Reset_filter_Dto')
        this.$store.commit('Reset_Search_Dto')
    },
};
</script>
