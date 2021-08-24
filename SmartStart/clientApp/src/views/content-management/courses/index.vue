<template>
    <div>
        <b-card no-body class="mb-1 ">
            <b-card-header>
                <strong>فلترة حسب</strong>
            </b-card-header>
            <b-card-body class="pb-1">
                <b-row>
                    <b-col cols="12" md="6" lg="3">
                        <EKInputSelect
                            label="الكلية"
                            placeholder="اختر الكلية "
                            :options="[{id: 0, name: 'الكل'}, ...faculties]"
                            name="select"
                            :clearable="true"
                            v-model="localeFilterDto.facultyId"
                        />
                    </b-col>
                    <b-col cols="12" md="6" lg="3">
                        <EKInputSelect
                            label="الفصل"
                            placeholder="اختر الفصل "
                            :options="[{id: 0, name: 'الكل'}, ...semester]"
                            name="select"
                            :clearable="true"
                            v-model="localeFilterDto.semesterId"
                        />
                    </b-col>
                    <b-col cols="12" md="6" lg="3">
                        <EKInputSelect
                            label="المادة"
                            placeholder="اختر المادة "
                            :options="[{id: 0, name: 'الكل'}, ...subjectsList]"
                            name="select"
                            :clearable="true"
                            v-model="localeFilterDto.subjectId"
                        />
                    </b-col>
                    <b-col cols="12" md="6" lg="3">
                        <EKInputSelect
                            label="سنة الدورة"
                            placeholder="اختر السنة "
                            :options="[{id: 0, name: 'الكل'}, ...years]"
                            name="year"
                            :clearable="true"
                            v-model="localeFilterDto.examYear"
                        />
                    </b-col>
                    <b-col cols="12" class="text-right">
                        <b-button type="submit" @click="$store.commit('Set_filter_Dto', localeFilterDto)" variant="primary">تم</b-button>
                    </b-col>
                </b-row>
            </b-card-body>
        </b-card>
        <EKTable 
            :items="courcesList"
            :columns="columns"
            selectedLabel="name"
              :row-style-class="rowStyleClassFn"
            @details="openCourcesDetails"
            @delete-selected="fireDeleteEvent"
        >
            <template slot="items.dateCreated" slot-scope="{ value }">
                {{ new Date(value).toLocaleDateString("en-GB") }}
            </template>
            <template slot="items.subject" slot-scope="{ value }">
                {{ value.name }}
            </template>
        </EKTable>  
    </div>
</template>
<script>
import EKTable from "@Ekcore/components/EK-table";
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";
import { mapActions, mapState,mapGetters } from "vuex";

export default {
    components: {
        EKTable,
        EKInputSelect
    },
    computed: {
        ...mapState({
            faculties: state => state.faculties.faculties,
            filterDto: state => state.filter.filterDto,
            subjectsList: state => state.subjects.subjectsList,
        }),
         ...mapGetters(["courcesList", "semester", "years"])
    },
    data: () => ({
        localeFilterDto: {
            subjectId: 0,
            examYear: 0,
            semesterId: 0,
            facultyId: 0
        },
        columns: [
            {
                label: "اسم الدورة",
                field: "name"
            },
            {
                label: "عدد الأسئلة",
                field: "questionsCount",
                type: 'number'
            },
            {
                label: "اسم المادة",
                field: "subject",
                sortable: false
            },
            {
                label: "تاريخ الإضافة",
                field: "dateCreated",
            },
            {
                label: "تفاصيل",
                field: "details",
                sortable: false
            }
        ]
    }),
    
    created() {
        this.getCourcesList(); 
        this.getFacultiesDetails()
        this.fetchTotalTag();
        this.fetchSubject();
    },
    methods: {
        ...mapActions(["getCourcesList","getFacultiesDetails", "fetchTotalTag", "fetchSubject"]),
        openCourcesDetails(props) {
            this.$router.push("/courses/" + props.row.id);
        },
        fireDeleteEvent(list) {
            console.log(list);
        },
          rowStyleClassFn(row) {
            return row.isFree ? 'bg-success' : '';
        },
    },
    beforeDestroy() {
        this.$store.commit('Reset_filter_Dto')
        this.$store.commit('Reset_Search_Dto')
    }
};
</script>
