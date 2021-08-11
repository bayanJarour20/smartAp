<template>
    <div>
        
        <b-form-checkbox-group v-model="selectednotifiactions">
            <b-card no-body class="mb-1">
                
                <b-card-header
                    class="p-0 px-1 justify-content-space-between d-flex"
                >
                    <div class="d-flex ">
                        
                        <small>تحديد الكل</small>
                    </div>
                    <b-button
                        size="sm"
                        variant="flat-secondary"
                        class="btn-icon rounded-circle"
                        style="padding: .5rem 10px!important"
                        @click="deletenafiticationList(selectednotifiactions)"
                    >
                        <unicon name="trash-alt" width="18"></unicon>
                    </b-button>
                </b-card-header>
            </b-card>
            <div
                v-for="(notifiactions, index) in activenotifiactionsList"
                :key="index"
            >
                <b-card
                    no-body
                    class="mb-1"
                >
                    <b-card-header class="pl-5 pr-1 pb-0 pt-50">
                        <span>{{ notifiactions.title }}</span>
                        <span class="ml-auto">{{
                            new Date(notifiactions.date).toLocaleDateString(
                                "EN-GB"
                            )
                        }}</span>
                        <b-button type="button" @click="getNotificationDetails(notifiactions)" variant="flat-secondary" class="ml-2 rounded-circle btn-icon">
                            <unicon name="ellipsis-v" width="18" height="18" />
                        </b-button>
                    </b-card-header>
                    <b-card-body class="p-1 d-flex align-items-center">
                        <b-form-checkbox
                            :value="notifiactions.id"
                        ></b-form-checkbox>
                        <p class="m-0">
                            {{ notifiactions.body.slice(0, 200)
                            }}{{ notifiactions.body.length > 200 ? "..." : "" }}
                        </p>
                    </b-card-body>
                </b-card>
            </div>
        </b-form-checkbox-group>
        <b-col cols="12" class="d-flex justify-content-center">
            <EKPagination
                :items="notificationList"
                v-model="activenotifiactionsList"
            />
        </b-col>
        <createNotifications ref="createNotifications" isEdit />
    </div>
</template>
<script>
import { mapActions, mapGetters, mapState } from "vuex";

import EKPagination from "@Ekcore/components/EK-pagination";
import createNotifications from "./components/create-notifications.vue";
export default {
    components: {
        EKPagination,
        createNotifications
    },
    data: () => ({
        selectednotifiactions: [],
        activenotifiactionsList: [],
    }),
    computed: {
        ...mapGetters(['notificationList']),
        ...mapState({
            notificationDto: state => state.natification.notificationDto,
        })
    },
    created() {
        this.getNotification();
    },
    methods: {
        ...mapActions(["getNotification","deletenafiticationList"]),
        getNotificationDetails(item) {
            this.$refs.createNotifications.open()
            Object.assign(this.notificationDto, item)
        }
    },
    beforeDestroy() {
        this.$store.commit('Reset_Search_Dto')
    },
};
</script>
<style lang="scss" scoped>
.filter-radius .custom-radio {
    display: inline-block;
    &:not(:first-of-type) {
        margin-left: 0.5rem;
    }
}
</style>
