<template>
    <div>
        <b-card no-body class="mb-1 ">
            <b-card-header class="py-1">
                <strong>فلترة حسب:</strong>
            </b-card-header>
            <b-card-body
                class="pb-0 px-0"
            >
                <b-col cols="12">
                    <b-row class="align-items-center">
                        <b-col cols="12" md="4" lg="3">
                            <EKInputSelect
                                label="السنة"
                                placeholder="اختر السنة "
                                :options="[{id: 0, name: 'الكل'}, ...years]"
                                name="examYear"
                                :clearable="true"
                                v-model="localeFilterDto.examYear"
                            />
                        </b-col>
                        <b-col cols="12" md="4" lg="3">
                            <EKInputSelect
                                label="الفصل"
                                placeholder="اختر الفصل "
                                :options="[{id: 0, name: 'الكل'}, ...semester]"
                                name="semesterId"
                                :clearable="true"
                                v-model="localeFilterDto.semesterId"
                            />
                        </b-col>
                        <b-col cols="12" md="4" lg="3">
                            <EKInputSelect
                                label="المادة"
                                placeholder="اختر المادة "
                                :options="[{id: 0, name: 'الكل'}, ...subjectsList]"
                                name="subjectId"
                                :clearable="true"
                                v-model="localeFilterDto.subjectId"
                            />
                        </b-col>
                        <b-col cols="12" lg="3" class="text-right">
                            <b-button
                                type="submit"
                                class="mb-1"
                                @click="getAllQuestion(localeFilterDto)"
                                variant="primary"
                                >تم</b-button
                            >
                        </b-col>
                    </b-row>
                </b-col>
            </b-card-body>
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
            @changeParentcheck="chaingeSelect"
            @changeSelectChildren="changeSelectChildren"
        >
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
            <EKPagination :items="questonsList" v-model="activeQuestionsList" />
        </b-col>
    </div>
</template>
<script>
import EKTableCollapse from "@Ekcore/components/EK-table-collapse";
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";
import { mapState, mapActions, mapGetters } from "vuex";
import EKPagination from "@Ekcore/components/EK-pagination";

export default {
    components: {
        EKTableCollapse,
        EKInputSelect,
        EKPagination
    },
    props: {
        id: String
    },
    data: () => ({
        header: [
            { label: "السؤال", value: "title" },
            { label: "مساعدة", value: "hint" },
            { label: "تاريخ الإضافة", value: "dateCreated" },
            { label: "التفاصيل", value: "details" }
        ],
        activeQuestionsList: [],
        questionHeader: [{label: "الجواب", value: "title"}],
        localeFilterDto: {
            examYear: 0,
            subjectId: 0,
            semesterId: 0
        }
    }),
    computed: {
        ...mapState({
            subjectsList: state => state.subjects.subjectsList
        }),
        ...mapGetters(["semester", "questonsList", "years"])
    },
    created() {
        this.getAllQuestion(this.localeFilterDto);
        this.fetchTotalTag();
        this.fetchSubject();
    },
    methods: {
        ...mapActions(["getAllQuestion", "fetchSubject", "fetchTotalTag"]),
        details(props) {
            this.$router.push(
                "/questions/" + props.answerType + "/set/" + props.id + "/0/0"
            );
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
