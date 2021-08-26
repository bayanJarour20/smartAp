<template>
    <div>
        <b-card no-body class="mb-1 ">
            <b-card-header class="py-1">
                <strong>
                    {{interviewQuestionList.name}}
                </strong>
                <b-button
                    class="ml-auto"
                    @click="deleteInterview(interviewQuestionList.id)"
                    variant="outline-danger"
                    >حذف السؤال الكتابي</b-button
                >
            </b-card-header>
        </b-card>
        <EKTableCollapse
            label="label"
            :rows="activeQuestionsList"
            :columns="header"
            childId="id"
            childrenLabel="answers"
            customHeaderLabel="label"
            :colapseHeader="questionHeader"
            @details="details"
            @changePareentcheck="chaingeSelect"
            @changeSelectChildren="changeSelectChildren"
        >
            <template slot="item-order" slot-scope="{ tr }">
                <b-button variant="primary" class="btn-icon rounded-pill">{{tr.order}}</b-button>
            </template>
            <template slot="sub-select-row" slot-scope="{tr, val}">
                <b-form-checkbox disabled class="mx-auto d-inline-block" :checked="tr.answers[val-1].isCorrect"></b-form-checkbox>
            </template>
            <template slot="item-dateCreated" slot-scope="{tr}">
                {{new Date(tr.dateCreated).toLocaleDateString('en-GB')}}
            </template>
        </EKTableCollapse>
        <b-col cols="12" class="d-flex justify-content-center mb-3">
            <EKPagination :items="searchedInterviewQuestionList" v-model="activeQuestionsList" />
        </b-col>
    </div>
</template>
<script>
import EKTableCollapse from "@Ekcore/components/EK-table-collapse";
import { mapActions, mapState, mapGetters } from 'vuex';
import EKPagination from "@Ekcore/components/EK-pagination";

export default {
    components: {
        EKTableCollapse,
        EKPagination
    },
    props: {
        id: String
    },
    data: () => ({
        activeQuestionsList: [],
        header: [
            {label: 'الترتيب', value: 'order'},
            {label: 'السؤال', value: 'title'},
            {label: 'مساعدة', value: 'hint'},
            {label: 'تاريخ الإضافة', value: 'dateCreated'},
            {label: 'التفاصيل', value: 'details'},
        ],
        questionHeader: [
            {label: 'الجواب', value: 'title'},
        ]
    }),
    computed: {
        ...mapState({
            interviewQuestionList: state => state.interviews.interviewQuestionList
        }),
        ...mapGetters(['searchedInterviewQuestionList'])
    },
    created() {
        this.fetchInterviewsDetails(this.id);
    },
    methods: {
        ...mapActions(["fetchInterviewsDetails", "deleteInterview"]),
        details(props) {
             this.$router.push("/questions/0/set/" + props.id + "/0/0");
        },
        chaingeSelect(list) {
            console.log(list);
        },
        changeSelectChildren(list) {
            console.log(list);
        }
    },
    beforeDestroy() {
        this.$store.commit('Reset_Search_Dto')
    },
};
</script>
