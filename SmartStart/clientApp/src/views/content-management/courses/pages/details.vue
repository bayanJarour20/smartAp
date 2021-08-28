<template>
    <div>
        <b-card no-body class="mb-1">
            <b-card-header class="py-1">
                <strong>
                    {{courcesQuestionList.name}}
                </strong>
                <b-button
                    class="ml-auto"
                    @click="deleteCourse(courcesQuestionList.id)"
                    variant="outline-danger"
                    >حذف الدورة</b-button
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
            no_delete
            :colapseHeader="questionHeader"
            @details="details"
        >
            <template slot="item-order" slot-scope="{ tr }">
                <b-button variant="primary" class="btn-icon rounded-pill">{{tr.order}}</b-button>
            </template>
            <template slot="sub-select-row" slot-scope="{ tr, val }">
                <b-form-checkbox
                    disabled
                    class="mx-auto d-inline-block"
                    :checked="tr.answers[val - 1].isCorrect"
                ></b-form-checkbox>
            </template>
            <template slot="item-dateCreated" slot-scope="{ tr }">
                {{ new Date(tr.dateCreated).toLocaleDateString("en-GB") }}
            </template>
        </EKTableCollapse>
        <b-col cols="12" class="d-flex justify-content-center mb-3">
            <EKPagination :items="searchedCourcesQuestionList" v-model="activeQuestionsList" />
        </b-col>
    </div>
</template>
<script>
import EKTableCollapse from "@Ekcore/components/EK-table-collapse";
import { mapActions, mapState, mapGetters } from "vuex";
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
            { label: "السؤال", value: "title" },
            { label: "مساعدة", value: "hint" },
            { label: "تاريخ الإضافة", value: "dateCreated" },
            { label: "التفاصيل", value: "details" }
        ],
        questionHeader: [{ label: "الجواب", value: "title" }]
    }),
    computed: {
        ...mapState({
            courcesQuestionList: state => state.cources.courcesQuestionList
        }),
        ...mapGetters(['searchedCourcesQuestionList'])
    },
    created() {
        this.getQuestionCources(this.id);
    },
    methods: {
        ...mapActions(["getQuestionCources", "deleteCourse"]),
        details(props) {
            this.$router.push("/questions/1/set/" + props.id + "/0/0");
        },
    },
    beforeDestroy() {
        this.$store.commit('Reset_Search_Dto')
    },
};
</script>
