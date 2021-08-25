<template>
    <div>
        <b-card no-body class="mb-1">
            <b-card-header class="pb-0">
                <strong>فلترة حسب</strong>
                <b-button
                    type="submit"
                    @click="$store.commit('Set_filter_Dto', filterSubjectDto)"
                    variant="primary"
                    >تم</b-button
                >
            </b-card-header>
            <b-card-body class="pb-50">
                <b-row align-v="center">
                    <b-col cols="12" md="4">
                        <EKInputSelect
                            label="الكلية"
                            placeholder="اختر الكلية "
                            :options="[{id: 0, name: 'الكل'}, ...faculties]"
                            name="select"
                            :clearable="true"
                            v-model="filterSubjectDto.facultyId"
                        />
                    </b-col>
                    <b-col cols="12" md="4">
                        <EKInputSelect
                            label="السنة"
                            placeholder="اختر السنة "
                            :options="[{id: 0, name: 'الكل'}, ...subjectYear]"
                            name="select"
                            :clearable="true"
                            v-model="filterSubjectDto.year"
                        />
                    </b-col>
                    <b-col cols="12" md="4">
                        <EKInputSelect
                            label="الفصل"
                            placeholder="اختر الفصل "
                            :options="[{id: 0, name: 'الكل'}, ...semester]"
                            name="select"
                            :clearable="true"
                            v-model="filterSubjectDto.semesterId"
                        />
                    </b-col>
                </b-row>
            </b-card-body>
        </b-card>
        <EKTable
            :items="subjectsList"
            :columns="columns"
            selectedLabel="id"
            @details="openSubjectDEtails"
            @delete-selected="subjectList"
        >
        </EKTable>
    </div>
</template>
<script>
import EKTable from "@Ekcore/components/EK-table";
import { mapActions, mapGetters, mapState } from "vuex";
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";

export default {
    components: {
        EKTable,
        EKInputSelect
    },
    data: () => ({
        columns: [
            {
                label: "اسم المادة",
                field: "name"
            },
           
            {
                label: "عدد الدورات",
                field: "examCount",
                type: 'number'
            },
            {
                label: "عدد البنوك",
                field: "bankCount",
                type: 'number'
            },
            {
                label: "عدد الأسئلة الكتابية",
                field: "interviewCount",
                type: 'number'
            },
            {
                label: "عدد المجاهر",
                field: "microscopeCount",
                type: 'number'
            },
            {
                label: "تفاصيل",
                field: "details",
                sortable: false
            }
        ],
        filterSubjectDto: {
            semesterId: 0,
            year: 0,
            facultyId: 0
        }
    }),
    computed: {
        ...mapState({
            faculties: state => state.faculties.faculties,
            subjectYear: state => state.globalStore.subjectYear
        }),
        ...mapGetters(["semester", "subjectsList"])
    },
    created() {
        this.fetchTotalTag();
        this.getFacultiesDetails()
        this.fetchSubject();
    },
    methods: {
        ...mapActions(["fetchSubject", "fetchTotalTag", "getFacultiesDetails","deleteSubjList"]),
        openSubjectDEtails(props) {
            this.$router.push("/subjects/" + props.row.id);
        },
        subjectList(list) {
            this.deleteSubjList(list)
        }
    },
    beforeDestroy() {
        this.$store.commit('Reset_filter_Dto')
        this.$store.commit('Reset_Search_Dto')
    }
};
</script>
