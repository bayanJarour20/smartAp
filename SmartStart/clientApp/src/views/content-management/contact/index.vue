// TODO : filters + multi delete
<template>
    <div>
        <!-- // selectedLabel --- opthinal props - default value: id -->
        <EKTable
            :items="feedbacks"
            :columns="columns"
            selectedLabel="name"
            @details="openContactDetails"
            @delete-selected="fireDeleteEvent"
        >
            <template slot="items.sendDate" slot-scope="{value}">
                {{ moment(value).format("MMMM Do YYYY, h:mm:ss a") }}
            </template>
        </EKTable>
    </div>
</template>
<script>
import EKTable from "@Ekcore/components/EK-table";
import { mapActions, mapState } from "vuex";
import moment from "moment";
export default {
    components: {
        EKTable
    },
    computed: {
        ...mapState({
            feedbacks: state => state.feedbacks.feedbacks
        })
    },
    data: () => ({
        columns: [
            {
                label: "العنوان",
                field: "title"
            },
            {
                label: "النص",
                field: "body",
                sortable: false
            },
            {
                label: "اسم الطالب",
                field: "appUserName"
            },
            {
                label: "تاريخ الإرسال",
                field: "sendDate"
            },
            {
                label: "تفاصيل",
                field: "details",
                sortable: false
            }
        ]
    }),
    created() {
        this.getFeedbackDetails();
    },
    methods: {
        ...mapActions(["getFeedbackDetails"]),
        openContactDetails(props) {
            this.$router.push("/contact/" + props.row.id);
        },
        moment,
        fireDeleteEvent(list) {
            console.log(list);
        }
    }
};
</script>
