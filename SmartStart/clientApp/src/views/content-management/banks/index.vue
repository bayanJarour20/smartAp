<template>
    <div>
        <b-card no-body class="mb-1">
            <b-card-header class="py-1">
                <strong>فلترة حسب:</strong>
            </b-card-header>
            <b-card-body class="pb-0 px-0">
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
                                label="الفصل"
                                placeholder="اختر الفصل"
                                v-model="localeFilterDto.semesterId"
                                :options="[
                                    { id: 0, name: 'الكل' },
                                    ...semester
                                ]"
                                name="semesterId"
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
                        <b-col cols="12" class="text-right">
                            <b-button
                                class="mb-1"
                                variant="primary"
                                @click="
                                    $store.commit(
                                        'Set_filter_Dto',
                                        localeFilterDto
                                    )
                                "
                                >فلترة</b-button
                            >
                        </b-col>
                    </b-row>
                </b-col>
            </b-card-body>
        </b-card>
        <EKTable
            :items="banksList"
            :columns="columns"
            @details="openBanksDetails"
            @delete-selected="fireDeleteEvent"
        >
            <template slot="items.dateCreated" slot-scope="{ value }">
                {{ new Date(value).toLocaleDateString("en-GB") }}
            </template>
        </EKTable>
    </div>
</template>
<script>
import EKTable from "@Ekcore/components/EK-table";
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";
import { mapState, mapActions, mapGetters } from "vuex";

export default {
    components: {
        EKTable,
        EKInputSelect
    },
    data: () => ({
        columns: [
            {
                label: "اسم البنك",
                field: "name"
            },
            {
                label: "عدد الأسئلة",
                field: "questionsCount",
                type: "number"
            },
            {
                label: "اسم المادة",
                field: "subjectName"
            },
            {
                label: "تاريخ الإضافة",
                field: "dateCreated"
            },
            {
                label: "تفاصيل",
                field: "details",
                sortable: false
            }
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
            faculties: state => state.faculties.faculties,
            filterDto: state => state.filter.filterDto,
            subjectsList: state => state.subjects.subjectsList
        }),
        ...mapGetters(["semester", "banksList", "years"])
    },
    created() {
        this.getBanksList();
        this.fetchTotalTag();
        this.fetchSubject();
        this.getFacultiesDetails();
    },
    methods: {
        ...mapActions([
            "getBanksList",
            "fetchSubject",
            "fetchTotalTag",
            "getFacultiesDetails",
            "deleteBankList"
        ]),
        fireDeleteEvent(list) {
            this.deleteBankList(list)
        },
        openBanksDetails(props) {
            this.$router.push("/banks/" + props.row.id);
        }
    },
    beforeDestroy() {
        this.$store.commit("Reset_filter_Dto");
        this.$store.commit("Reset_Search_Dto");
    }
};
</script>
