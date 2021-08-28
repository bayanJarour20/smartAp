<template>
    <div>
        <EKTable
            :items="usersList"
            :columns="columns[type]"
            selectedLabel="id"
            @details="openUsersDetails"
            @delete-selected="usersDeleteEvent"
        >
            <template slot="items.dateBlocked" slot-scope="{ value }">
                <b-badge :variant="value ? 'danger' : 'success'">{{
                    value ? "غير مفعل" : "مفعل"
                }}</b-badge>
            </template>
            <template slot="items.dateCreated" slot-scope="{ value }">
                {{ moment(new Date(value)).format("YYYY/MM/DD HH:mm:ss") }}
            </template>
            <template slot="items.subscriptionDate" slot-scope="{ value }">
                {{ moment(new Date(value)).format("YYYY/MM/DD HH:mm:ss") }}
            </template>
            <template
                v-if="type == 0"
                slot="items.facultiesIds"
                slot-scope="{ value, props }"
            >
                <div :id="'popover-target' + props.row.id">
                    {{
                        value == null || value.length == 0
                            ? "-"
                            : value.length == 1
                            ? facultiesMap.get(value[0])
                            : facultiesMap.get(value[0]) + "..."
                    }}
                </div>
                <b-popover
                    v-if="value != null && value.length > 0"
                    :target="'popover-target' + props.row.id"
                    triggers="hover"
                    placement="top"
                >
                    {{ formatFaculties(value) }}
                </b-popover>
            </template>
        </EKTable>
    </div>
</template>
<script>
import EKTable from "@Ekcore/components/EK-table";
import { mapGetters, mapActions } from "vuex";
import moment from "moment";
export default {
    props: {
        lang: String
    },
    components: {
        EKTable
    },
    computed: {
        ...mapGetters(["usersList", "facultiesMap"]),
        type() {
            switch (this.$route.name) {
                case "application-users":
                    return 0;
                case "point-sales":
                    return 1;
                case "panel-users":
                    return 2;
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
                    label: "E-mail",
                    field: "email",
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
                    label: "E-mail",
                    field: "email",
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
                    label: "E-mail",
                    field: "email",
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
        ]
    }),
    created() {
        this.getUsers(this.type);
        this.getFacultiesDetails();
    },
    methods: {
        ...mapActions(["getUsers", "getFacultiesDetails", "deleteUsersList"]),
        moment,
        openUsersDetails(props) {
            this.$router.push(
                "/users/" + this.type + "/details/" + props.row.id
            );
        },
        usersDeleteEvent(list) {
            this.deleteUsersList(list);
        },
        formatFaculties(arr) {
            var s = "";
            arr.forEach((element, i) => {
                s += this.facultiesMap.get(element);
                if (i != arr.length - 1) {
                    s += "، ";
                } else {
                    s += ".";
                }
            });
            return s;
        }
    },
    watch: {
        type(type) {
            this.$store.commit("Reset_Search_Dto");
            this.getUsers(type);
        }
    },
    beforeDestroy() {
        this.$store.commit("Reset_Search_Dto");
    }
};
</script>
