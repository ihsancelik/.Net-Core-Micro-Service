<template>
  <ValidationObserver ref="observer">
    <v-container style="width: 1200px; display: grid">
      <v-card class="pa-10 pt-5" style="display: inline !important" elevation="5">
        <a @click="backTo()" large>
          <v-icon style="float: left"> mdi-keyboard-backspace </v-icon>
        </a>

        <h2 class="text-md-center" style="font-family: math; font-size: 2em">
          {{ translator("ticketMessageDetails") }}
        </h2>

        <v-btn
          v-if="this.ticketGroupModel.isCanceled"
          @click="showReply"
          medium
          color="success"
          style="float: right; width: 20%"
        >
          {{ translator("answer") }}
        </v-btn>
        <hr class="mb-3 mt-12" />

        <v-row v-for="item in orderedTicketMessage" :key="item.id" style="display: inline !important">
          <!-- Customer -->
          <v-col cols="12" v-if="!item.isAdmin" class="customer mt-3" style="float: right !important">
            <div>
              <v-icon style="float: left" class="mr-3"> mdi-account </v-icon>
              <b>{{ item.userName }}</b>
              <p style="float: right">
                <b> {{ new Date(item.createdDate).toISOString().substr(0, 10).replace("T", " ") }} </b>
                {{ new Date(item.createdDate).toISOString().substr(10, 6).replace("T", " ") }}
                <v-icon v-if="item.imageName != null" @click="openImage(item.id)" class="mr-2">
                  mdi-image-filter-hdr
                </v-icon>
              </p>
            </div>
            <div class="mt-5">
              <p class="mt-5">{{ item.message }}</p>
            </div>
          </v-col>

          <!-- Admin -->
          <v-col cols="12" v-else class="adminReply mt-3" style="float: left !important">
            <div>
              <v-icon style="float: left" class="mr-2"> mdi-account-edit </v-icon>
              <b>{{ item.adminName }}</b>
              <p style="float: right">
                <b> {{ new Date(item.createdDate).toISOString().substr(0, 10).replace("T", " ") }} </b>
                {{ new Date(item.createdDate).toISOString().substr(10, 6).replace("T", " ") }}
                <v-icon v-if="item.imageName != null" @click="openImage(item.id)" class="mr-2">
                  mdi-image-filter-hdr
                </v-icon>
              </p>
            </div>
            <div style="margin-top: 2%">
              <p class="mt-2">{{ item.message }}</p>
            </div>
          </v-col>
        </v-row>
        <br />
      </v-card>
    </v-container>

    <v-dialog v-model="dialogEnable" :max-width="dialogWidth" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog" />
    </v-dialog>
  </ValidationObserver>
</template>

<script>
/* eslint-disable */
import * as rules from "vee-validate/dist/rules";
import { extend, ValidationObserver, ValidationProvider, setInteractionMode } from "vee-validate";
import TicketService from "@/services/TicketService";
import TicketMessageReply from "./TicketMessageReply";
import OpenImage from "./OpenImage";

setInteractionMode("eager");
extend("required", { ...rules.required, message: "{_field_} can not be empty" });
extend("max", { ...rules.max, message: "{_field_} may not be greater than {length} characters" });

export default {
  components: {
    ValidationObserver,
    ValidationProvider,
  },
  data() {
    return {
      ticketService: new TicketService(),

      ticketGroups: [],
      groupId: 0,

      ticketGroupModel: {
        isAdmin: false,
        userName: "",
        company: "",
        isClosed: true,
      },

      signalModel: {
        changes: false,
        returnValues: null,
      },

      dialogEnable: false,
      dialogKey: 0,
      dialogWidth: null,
      selectedComponent: "",
      componentProps: null,
    };
  },
  created() {
    this.groupId = this.$store.getters.getTicketGroupId;

    this.initialize();
    let that = this;
    document.addEventListener("keyup", function (evt) {
      if (evt.keyCode === 27) {
        that.close();
      }
    });
  },
  methods: {
    async initialize() {
      this.ticketGroups = [];

      let isCancel = await new TicketService().getGroupCloseInfo(this.groupId);
      this.ticketGroupModel.isCanceled = isCancel.success;

      this.ticketService.getTicketMessages(this.groupId).then((ticketMsgs) => {
        ticketMsgs.list.forEach((element) => {
          this.ticketGroups.push(element);
        });
      });
    },
    showReply() {
      this.selectedComponent = TicketMessageReply;
      this.componentProps = {
        groupIdProps: this.groupId,
      };
      this.openDialog(600);
    },
    openImage(id) {
      this.selectedComponent = OpenImage;
      this.componentProps = {
        messageIdProp: id,
      };
      this.openDialog(1000);
    },
    backTo() {
      this.$router.push({
        path: "/user/ticket-req",
      });
    },
    openDialog(width = 500) {
      this.dialogEnable = true;
      this.dialogWidth = width;
    },
    closeDialog() {
      this.dialogEnable = false;
      this.dialogKey += 1;
      this.initialize();
    },
    close(changes) {
      this.signalModel.changes = changes;
      this.$emit("closed", this.signalModel);
    },
  },
  computed: {
    orderedTicketMessage() {
      return _.orderBy(this.ticketGroups, ["createdDate"], ["asc"]);
    },
  },
};
</script>

<style scoped>
.adminReply {
  font-family: "" Roboto ",sans-serif !important";
  font-size: 16px;
  letter-spacing: -0.3px;
  border-radius: 10px 10px 10px 0px;
  background-color: rgb(231, 241, 174);
  padding: 2%;
  max-width: 60%;
}

.customer {
  border-radius: 10px 10px 0px 10px;
  font-family: "" Roboto ",sans-serif !important";
  font-size: 16px;
  max-width: 60%;
  margin-bottom: 20px;
  padding: 2%;
  background-color: rgb(195, 227, 240);
}

.answer {
  position: relative;
  margin-top: 30%;
  margin-left: 10%;
  width: 30%;
}
</style>
