import Vue from "vue";
import FeatherIcon from "@core/components/feather-icon/FeatherIcon.vue";
import Unicon from "vue-unicons";
import {
    uniMoon,
    uniBell,
    uniCircle,
    uniSun,
    uniPlus,
    uniHomeAlt,
    uniCalendarAlt,
    uniUser,
    uniDesktop,
    uniHorizontalAlignRight,
    uniMap,
    uniNewspaper,
    uniServerAlt,
    uniLayerGroup,
    uniBuilding,
    uniFileQuestionAlt,
    uniBagAlt,
    uniTelescope,
    uniChatBubbleUser,
    uniQrcodeScan,
    uniBill,
    uniUsersAlt,
    uniUserSquare,
    uniCog,
    uniBooks,
    uniFileAlt,
    uniEnvelopeAlt,
    uniMoneyWithdrawal
} from "vue-unicons/src/icons";

Unicon.add([
    // for theme
    uniPlus,
    uniLayerGroup,
    uniServerAlt,
    uniDesktop,
    uniNewspaper,
    uniMap,
    uniSun,
    uniHorizontalAlignRight,
    uniMoon,
    uniBell,
    uniCircle,
    uniHomeAlt,
    uniCalendarAlt,
    uniUser,
    // end
    uniBuilding,
    uniBooks,
    uniFileQuestionAlt,
    uniFileAlt,
    uniBagAlt,
    uniTelescope,
    uniChatBubbleUser,
    uniEnvelopeAlt,
    uniDesktop,
    uniQrcodeScan,
    uniBill,
    uniUsersAlt,
    uniMoneyWithdrawal,
    uniUserSquare,
    uniCog
]);
Vue.use(Unicon);
Vue.component(FeatherIcon.name, FeatherIcon);
