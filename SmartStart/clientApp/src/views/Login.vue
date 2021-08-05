<template>
    <div class="auth-wrapper auth-v2">
        <b-row class="auth-inner m-0">
            <!-- Brand logo-->
            <b-link class="brand-logo">
                <vuexy-logo />
                <h2 class="brand-text text-primary ml-1 mb-0 d-flex align-items-center">
                    Smart Start
                </h2>
            </b-link>
            <!-- /Brand logo-->

            <!-- Left Text-->
            <b-col lg="8" class="d-none d-lg-flex align-items-center p-5">
                <div
                    class="w-100 d-lg-flex align-items-center justify-content-center px-5"
                >
                    <b-img fluid :src="imgUrl" alt="Login V2" />
                </div>
            </b-col>
            <!-- /Left Text-->

            <!-- Login-->
            <b-col lg="4" class="d-flex align-items-center auth-bg px-2 p-lg-5">
                <b-col sm="8" md="6" lg="12" class="px-xl-2 mx-auto">
                    <b-card-title title-tag="h2" class="font-weight-bold mb-1">
                        مرحبا" بكم في Smart Start! 👋
                    </b-card-title>
                    <b-card-text class="mb-2">
                        رجاء سجل الدخول بحسابك لتبدأ الإستكشاف
                    </b-card-text>

                    <!-- form -->
                    <validation-observer ref="loginValidation">
                        <b-form class="auth-login-form mt-2" ref="loginForm" @submit.prevent>
                            <!-- username -->
                            <b-form-group label="اسم المستخدم" label-for="login-username">
                                <validation-provider
                                    #default="{ errors }"
                                    name="الحقل اسم المستخدم"
                                    rules="required"
                                >
                                    <b-form-input
                                        id="login-username"
                                        v-model="username"
                                        :state="
                                            errors.length > 0 ? false : null
                                        "
                                        name="login-username"
                                        placeholder="username"
                                    />
                                    <small class="text-danger">{{
                                        errors[0]
                                    }}</small>
                                </validation-provider>
                            </b-form-group>

                            <!-- forgot password -->
                            <b-form-group>
                                <div class="d-flex justify-content-between">
                                    <label for="login-password">كلمة المرور</label>
                                </div>
                                <validation-provider
                                    #default="{ errors }"
                                    name="الحقل كلمة المرور"
                                    rules="required"
                                >
                                    <b-input-group
                                        class="input-group-merge"
                                        :class="
                                            errors.length > 0
                                                ? 'is-invalid'
                                                : null
                                        "
                                    >
                                        <b-form-input
                                            id="login-password"
                                            v-model="password"
                                            :state="
                                                errors.length > 0 ? false : null
                                            "
                                            class="form-control-merge"
                                            :type="passwordFieldType"
                                            name="login-password"
                                            placeholder="············"
                                        />
                                        <b-input-group-append is-text>
                                            <feather-icon
                                                class="cursor-pointer"
                                                :icon="passwordToggleIcon"
                                                @click="
                                                    togglePasswordVisibility
                                                "
                                            />
                                        </b-input-group-append>
                                    </b-input-group>
                                    <small class="text-danger">{{
                                        errors[0]
                                    }}</small>
                                </validation-provider>
                            </b-form-group>

                            <!-- checkbox -->
                            <b-form-group>
                                <b-form-checkbox
                                    id="remember-me"
                                    v-model="status"
                                    name="checkbox-1"
                                >
                                    تذكرني
                                </b-form-checkbox>
                            </b-form-group>

                            <!-- submit buttons -->
                            <b-button
                                type="submit"
                                variant="primary"
                                block
                                @click="login"
                            >
                                تسجيل الدخول
                            </b-button>
                        </b-form>
                    </validation-observer>
                </b-col>
            </b-col>
            <!-- /Login-->
        </b-row>
    </div>
</template>

<script>
import { ValidationProvider, ValidationObserver, localize } from "vee-validate";
import VuexyLogo from "@core/layouts/components/Logo.vue";
import {
    BRow,
    BCol,
    BLink,
    BFormGroup,
    BFormInput,
    BInputGroupAppend,
    BInputGroup,
    BFormCheckbox,
    BCardText,
    BCardTitle,
    BImg,
    BForm,
    BButton
} from "bootstrap-vue";
import useJwt from "@/auth/jwt/useJwt";
import { required } from "@validations";
import { togglePasswordVisibility } from "@core/mixins/ui/forms";
import ToastificationContent from "@core/components/toastification/ToastificationContent.vue";
import store from "@/store/index";
import { getHomeRouteForLoggedInUser } from "@/auth/utils";
import jwtDecode from "jwt-decode";

export default {
    components: {
        BRow,
        BCol,
        BLink,
        BFormGroup,
        BFormInput,
        BInputGroupAppend,
        BInputGroup,
        BFormCheckbox,
        BCardText,
        BCardTitle,
        BImg,
        BForm,
        BButton,
        VuexyLogo,
        ValidationProvider,
        ValidationObserver
    },
    mixins: [togglePasswordVisibility],
    data() {
        return {
            status: false,
            password: "",
            username: "",
            sideImg: require("@/assets/images/pages/login-v2.svg"),
            required
        };
    },
    created() {
        localize('ar')
    },
    computed: {
        passwordToggleIcon() {
            return this.passwordFieldType === "password"
                ? "EyeIcon"
                : "EyeOffIcon";
        },
        imgUrl() {
            if (store.state.appConfig.layout.skin === "dark") {
                // eslint-disable-next-line vue/no-side-effects-in-computed-properties
                this.sideImg = require("@/assets/images/pages/login-v2-dark.svg");
                return this.sideImg;
            }
            return this.sideImg;
        }
    },
    methods: {
        login() {
            this.$refs.loginValidation.validate().then(success => {
                if (success) {
                    useJwt
                        .login({
                            username: this.username,
                            password: this.password,
                            rememberMe: this.status
                        })
                        .then(response => {
                            const userData = response.data;
                            useJwt.setToken(response.data.token);
                            useJwt.setRefreshToken(response.data.refreshToken);
                            localStorage.setItem(
                                "userData",
                                JSON.stringify(userData)
                            );
                            // ? This is just for demo purpose. Don't think CASL is role based in this case, we used role in if condition just for ease
                            this.$router
                                .replace(
                                    getHomeRouteForLoggedInUser(jwtDecode(response.data.token)['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'])
                                )
                                .then(() => {
                                    this.$toast({
                                        component: ToastificationContent,
                                        position: "top-right",
                                        props: {
                                            title: `مرحبا" ${userData.firstName + userData.lastName || userData.userName}`,
                                            icon: "CoffeeIcon",
                                            variant: "success",
                                            text: `تم تسجيل الدخول بنجاح. الأن يمكنك التصفح!`
                                        }
                                    });
                                })
                                .catch(error => {
                                    console.log(error)
                                });
                        }).catch((err) => {
                            console.log(err)
                        })
                }
            });
        }
    }
};
</script>

<style lang="scss">
@import "@core/scss/vue/pages/page-auth.scss";
</style>
