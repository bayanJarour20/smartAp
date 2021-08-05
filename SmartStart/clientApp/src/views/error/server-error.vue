<template>
    <div class="flex-column d-flex justify-content-center align-items-center" style="height: 100vh">
        <h1 class="pb-1" >خطأ في الإتصال</h1>
        <b-button variant="primary" to="/">
            عودة للرئيسية
        </b-button>
    </div>
</template>

<script>
import { BButton } from "bootstrap-vue";
export default {
    components: {
        BButton
    }
}
</script>