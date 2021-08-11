<template>
    <ValidationObserver ref="observer">
        <EKDialog
            ref="notificationDialog"
            :title="!isEdit ? 'إرسال إشعار' : 'تفاصيل الإشعار'"
            :placeholder="!isEdit ? 'ابحث عن إشعار ما...' : ''"
            :btnText="!isEdit ? 'إرسال إشعار' : ''"
           
            @close="$store.commit('Reset_Notification_Dto')"
            :isEdit="isEdit"
            
            @search="search"
        >
            <template #body>
                <div v-if="!isEdit">
                    <label>إرسال إلى</label>
                    <div class="d-flex align-items-center my-50">
                        <label class="m-0">الطلاب</label>
                        <b-checkbox @change="notificationDto.userIds = []" switch v-model="notificationDto.notificationType"></b-checkbox>
                        <label class="m-0">نقاط البيع</label>
                    </div>
                </div>
                <label class="mb-50">فلترة حسب : </label>
                <EKInputSelect
                    v-if="!notificationDto.notificationType"
                    label="كلية"
                    placeholder="اختر الكلية"
                    :options="faculties"
                    name="selectfactely"
                    :clearable="true"
                    v-model="notificationFilterDto.faculityId"
                />
                <EKInputSelect
                    v-else
                    label="المدينة"
                    placeholder="اختر المدينة"
                    :options="citiesList"
                    name="selectciies"
                    v-model="notificationFilterDto.cityId"
                    :clearable="true"
                />
                <label v-if=" filterdUserListForNotification!=0 ">إرسال إلى</label>
                    <div v-if=" filterdUserListForNotification!=0 " class="d-flex align-items-center my-50">                      
                        <b-checkbox  v-model="type"></b-checkbox>
                        <label class="m-0" v-if="!notificationDto.notificationType">جميع الطلاب</label>
                        <label class="m-0" v-if="notificationDto.notificationType">جميع نقاط البيع</label>
                    </div>
                <EKInputSelect
                    v-if="!notificationDto.notificationType && !type && filterdUserListForNotification!=0"
                    label="الطالب"
                    placeholder="اختر الطالب"
                    name="selectstudent"
                    :options="filterdUserListForNotification"
                    multiple
                    v-model="notificationDto.userIds"
                    :clearable="true"
                />
                <EKInputSelect
                    v-if="notificationDto.notificationType && !type && filterdUserListForNotification!=0 "
                    label="نقطة البيع"
                    placeholder="اختر نقطة البيع"
                    name="selectsell"
                    multiple
                    :options="filterdUserListForNotification"
                    v-model="notificationDto.userIds"
                    :clearable="true"
                />
                  <label class="text-danger" v-if="filterdUserListForNotification==0  && !notificationDto.notificationType " > لا يوجد ولا طالب للإرسال له</label>
                  <label class="text-danger" v-if="filterdUserListForNotification==0  && notificationDto.notificationType " > لا يوجد ولا نقطة بيع للإرسال لها</label>
                  <label class="text-danger" v-if="notificationDto.userIds.length==0 && !notificationDto.notificationType && filterdUserListForNotification!=0  && !type"> الرجاء اختيار طلاب محددين</label>
                  <label class="text-danger" v-if="notificationDto.userIds.length==0 && notificationDto.notificationType && filterdUserListForNotification!=0 && !type"> الرجاء اختيار نقطة بيع محددة</label>
                <hr>
               
                <EKInputText
                    :rules="[{ type: 'required', message: ' العنوان مطلوب' }]"
                    label="عنوان الإشعار"
                    v-model="notificationDto.title"
                    placeholder="ادخل العنوان"
                    name="title"
                />
                <EKInputTextarea
                    :rules="[{ type: 'required', message: ' النص إجباري' }]"
                    label="نص الإشعار"
                    v-model="notificationDto.body"
                    placeholder="ادخل النص"
                    name="body"
                />
            </template>
            <template #footer>
            <slot name="footer">
                <div class="d-flex align-items-center px-1 py-1 border-top">
                    <b-button variant="outline-danger" class="mr-auto" v-if="isEdit" @click="deleteNotification(notificationDto.id)">حذف</b-button>
                    <b-button
                        variant="outline-secondary"
                        class=" "
                        @click="close()"
                        >إلغاء</b-button
                    >
                    <b-button variant="primary" class="ml-auto" v-if="!isEdit" @click="onSubmit">حفظ</b-button>
                </div>
            </slot>
        </template>
        </EKDialog>
    </ValidationObserver>
</template>
<script>
import { ValidationObserver } from "vee-validate";
import EKDialog from "@Ekcore/components/EK-dialog";
import EKInputText from "@Ekcore/components/EK-forms/EK-input-text";
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";
import EKInputTextarea from "@Ekcore/components/EK-forms/EK-input-textarea";
import { mapActions, mapState, mapGetters } from "vuex";

export default {
    props: {
        isEdit: Boolean
    },
    components: {
        ValidationObserver,
        EKDialog,
        EKInputText,
        EKInputSelect,
        EKInputTextarea
    },
    data() {
        return {
            currentState: false,
            type:false
        };
    },
    computed: {
        ...mapGetters(["filterdUserListForNotification", "tagsList", "semester", "doctors"]),
        ...mapState({
            notificationDto: state => state.natification.notificationDto,
            notificationFilterDto: state => state.natification.notificationFilterDto,
            faculties: state => state.faculties.faculties,
            citiesList: state => state.globalStore.citiesList
        })
    },
    created() {
        this.getFacultiesDetails()
        this.fetchCity()
        this.fetchTotalTag()
        this.getUsers(0)
       
    },
    methods: {
        ...mapActions(["deleteNotification", "newNotification", "getFacultiesDetails", "fetchCity", "fetchTotalTag", "getUsers"]),
        onSubmit() {
            this.$refs.observer.validate().then(success => {
                if (success && this.filterdUserListForNotification!=0 && this.notificationDto.userIds.length!=0) {
                    this.newNotification({
                        title: this.notificationDto.title,
                        body: this.notificationDto.body,
                        date: this.notificationDto.date,
                        notificationType: +this.notificationDto.notificationType,
                        userIds: this.notificationDto.userIds
                    });
                }
            });
        },
        open() {
            this.$refs.notificationDialog.open()
        },
        close() {
            this.$refs.notificationDialog.close()
        },
        search(query) {
            this.$store.commit('Set_Search_Dto', {
                keys: ["title", 'body'],
                query   
            })
        }
    },
    watch: {
        'notificationDto.notificationType'(val) {
            this.notificationFilterDto.faculityId = 0
            this.notificationFilterDto.cityId = 0
            this.getUsers(+val)
        },
        'notificationFilterDto.faculityId'(){
            this.notificationDto.userIds=[]

        }
    }
};
</script>
