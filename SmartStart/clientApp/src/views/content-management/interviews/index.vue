<template>
    <div>
        <b-card no-body class="mb-1">
            <b-card-header class="py-1">
                <strong>فلترة حسب:</strong>
            </b-card-header>
            <b-card-body class="pb-0 px-1">
                <b-col cols="12">
                    <b-row class="align-items-end">
                        <b-col cols="12" md="4" lg="3">
                            <EKInputSelect
                                label="الكلية"
                                placeholder="اختر الكلية "
                                :options="[
                                    { id: 0, name: 'الكل' },
                                    ...faculties
                                ]"
                                name="facultyId"
                                :clearable="true"
                                v-model="localeFilterDto.facultyId"
                            />
                        </b-col>
                        <b-col cols="12" md="4" lg="3">
                            <EKInputSelect
                                label="السنة"
                                placeholder="اختر السنة"
                                v-model="localeFilterDto.examYear"
                                :options="[{ id: 0, name: 'الكل' }, ...years]"
                                name="examYear"
                                :clearable="true"
                            />
                        </b-col>
                        <b-col cols="12" md="4" lg="3">
                            <EKInputSelect
                                label="المادة"
                                placeholder="اختر المادة"
                                v-model="localeFilterDto.subjectId"
                                :options="[
                                    { id: 0, name: 'الكل' },
                                    ...subjectsList
                                ]"
                                name="subjectId"
                                :clearable="true"
                            />
                        </b-col>
                        <b-col cols="12" md="4" lg="3">
                            <EKInputSelect
                                label="الفصل"
                                placeholder="اختر الفصل"
                                v-bind="localeFilterDto.semesterId"
                                :options="[
                                    { id: 0, name: 'الكل' },
                                    ...semester
                                ]"
                                name="semesterId"
                                :clearable="true"
                            />
                        </b-col>
                        <b-col cols="12" class="text-right">
                            <b-button
                                type="submit"
                                variant="primary"
                                class="mb-1"
                                @click="
                                    $store.commit(
                                        'Set_filter_Dto',
                                        localeFilterDto
                                    )
                                "
                                >تم</b-button
                            >
                        </b-col>
                    </b-row>
                </b-col>
            </b-card-body>
        </b-card>
        <EKTable
            :items="interviewsList"
            :columns="header"
            @details="openInterviewsDetails"
            @delete-selected="fireDeleteEvent"
        >
            <template slot="items.dateCreated" scope="{value}">
                {{ new Date(value).toLocaleDateString("en-GB") }}
            </template>
        </EKTable>
    </div>
</template>
<script>
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";
import { mapState, mapActions, mapGetters } from "vuex";
import EKTable from "@Ekcore/components/EK-table";

export default {
    components: {
        EKTable,
        EKInputSelect
    },
    data: () => ({
        header: [
            { label: "العنوان", field: "name" },
            { label: "عدد الأسئلة", field: "questionsCount", type: "number" },
            { label: "اسم المادة", field: "subjectName" },
            { label: "تاريخ الإضافة", field: "dateCreated" },
            { label: "التفاصيل", field: "details", sortable: false }
        ],
        localeFilterDto: {
            examYear: 0,
            subjectId: 0,
            facultyId: 0,
            semesterId: 0
        }
    }),
    computed: {
        ...mapState({
            subjectsList: state => state.subjects.subjectsList,
            faculties: state => state.faculties.faculties
        }),
        ...mapGetters(["semester", "interviewsList", "years"])
    },
    created() {
        this.fetchInterviews();
        this.fetchTotalTag();
        this.fetchSubject();
        this.getFacultiesDetails();
    },
    methods: {
        ...mapActions([
            "fetchInterviews",
            "fetchTotalTag",
            "fetchSubject",
            "getFacultiesDetails",
            "deleteInterviewList"
        ]),
        fireDeleteEvent(list) {
            this.deleteInterviewList(list)
        },
        openInterviewsDetails(props) {
            this.$router.push("/interviews/" + props.row.id);
        }
    },
    rowStyleClassFn(row) {
        return row.isFree ? "bg-success" : "";
    },
    beforeDestroy() {
        this.$store.commit("Reset_filter_Dto");
        this.$store.commit("Reset_Search_Dto");
    }
};
</script>
