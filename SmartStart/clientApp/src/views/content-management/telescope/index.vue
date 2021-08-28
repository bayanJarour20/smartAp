<template>
    <div>
        
        <b-card no-body class="mb-1">
            <b-card-header class="pb-0">
                <strong>فلترة حسب</strong> 
                <b-button
                    variant="primary"
                    @click="$store.commit('Set_filter_Dto', localeFilterDto)"
                    >تم</b-button
                >
            </b-card-header>
            <b-card-body class="p-0">
                <b-col>
                    <b-row>
                        <b-col cols="12" md="6">
                            <EKInputSelect
                                label="السنة"
                                placeholder="اختر السنة"
                                :options="[{id: 0, name: 'الكل'}, ...years]"
                                v-model="localeFilterDto.examYear"
                                name="examYear"
                                :clearable="true"
                            />
                        </b-col>
                        <b-col cols="12" md="6">
                            <EKInputSelect
                                label="الفصل"
                                placeholder="اختر الفصل"
                                :options="[{id: 0, name: 'الكل'}, ...semester]"
                                name="semester"
                                v-model="localeFilterDto.semesterId"
                                :clearable="true"
                            />
                        </b-col>
                    </b-row>               
                </b-col>
            </b-card-body>
        </b-card>
        <EKTable
            :items="telescopeList"
            :columns="columns"
            @details="openSubjectDEtails"
            @delete-selected="fireDeleteEvent"
        >
            <template slot="items.dateCreated" scope="{value}">
                {{new Date(value).toLocaleDateString('en-GB')}}
            </template>
        </EKTable>
    </div>
</template>
<script>
import EKTable from "@Ekcore/components/EK-table";
import { mapActions, mapGetters } from 'vuex';
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";
export default {
    components: {
        EKTable,
        EKInputSelect
    },
    data: () => ({
        columns: [
            {
                label: "اسم المحاضرة",
                field: "name"
            },
            {
                label: "عدد الأسئلة",
                field: "questionsCount",
                type: "number",
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
            semesterId: 0
        }
    }),
    computed: {
        ...mapGetters(["semester", "telescopeList", 'years']),
    },
    created() {
        this.microscopeGetAll()
        this.fetchTotalTag();
    },
    methods: {
        ...mapActions(['microscopeGetAll', "fetchTotalTag", "deleteMicroscopeList"]),
        openSubjectDEtails(props) {
            this.$router.push("/telescope/" + props.row.id);
        },
        fireDeleteEvent(list) {
            this.deleteMicroscopeList(list)
        }
    },
    beforeDestroy() {
        this.$store.commit('Reset_filter_Dto')
        this.$store.commit('Reset_Search_Dto')
    }
};
</script>
 
