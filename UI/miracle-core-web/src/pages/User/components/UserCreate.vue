<template>
  <ValidationObserver ref="observer">
    <v-card class="pa-10 pt-5" elevation="5">
      <a @click="close(false)">
        <v-icon style="float: right;">mdi-close-circle</v-icon>
      </a>
      <h2 class="text-md-center">{{ translator("newUser") }}</h2>
      <hr class="mb-3" />

      <v-form>
        <v-row class="rows1">
          <v-col cols="12" md="8">
            <v-file-input
              prepend-icon=""
              prepend-inner-icon="mdi-camera"
              accept=".jpg,.png,.jpeg"
              :placeholder="translator('userImage')"
              @change="uploadImage"
            ></v-file-input>
          </v-col>
          <v-col cols="12" md="4">
            <img id="vimg" src="/miracle-logo.png" class="profileImg" />
          </v-col>
        </v-row>

        <v-row class="rows">
          <v-col cols="12" md="6">
            <ValidationProvider #default="{ errors }" :name="translator('name')" rules="required|max:64">
              <v-text-field
                v-model="model.userModel.name"
                :label="translator('name')"
                :error-messages="errors"
                outlined
                counter="64"
              ></v-text-field>
            </ValidationProvider>
          </v-col>

          <v-col cols="12" md="6">
            <ValidationProvider
              #default="{ errors }"
              :name="translator('surname')"
              rules="required|max:64"
            >
              <v-text-field
                v-model="model.userModel.surname"
                :label="translator('surname')"
                :error-messages="errors"
                outlined
                counter="64"
              ></v-text-field>
            </ValidationProvider>
          </v-col>
        </v-row>

        <v-row class="rows">
          <v-col cols="12" md="12">
            <ValidationProvider
              #default="{ errors }"
              :name="translator('username')"
              rules="required|max:32"
            >
              <v-text-field
                v-model="model.userModel.username"
                :label="translator('username')"
                :error-messages="errors"
                outlined
                counter="32"
              ></v-text-field>
            </ValidationProvider>
          </v-col>
        </v-row>

        <v-row class="rows">
          <v-col cols="12" md="6">
            <ValidationProvider
              #default="{ errors }"
              :name="translator('password')"
              rules="required|min:8"
              vid="password"
            >
              <v-text-field
                v-model="model.userModel.password"
                :label="translator('password')"
                :error-messages="errors"
                outlined
                :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
                :type="showPassword ? 'text' : 'password'"
                @click:append="showPassword = !showPassword"
              ></v-text-field>
            </ValidationProvider>
          </v-col>

          <v-col cols="12" md="6">
            <ValidationProvider
              #default="{ errors }"
              :name="translator('rePassword')"
              rules="required|confirmed:password|min:8"
            >
              <v-text-field
                v-model="model.userModel.repassword"
                :label="translator('rePassword')"
                :error-messages="errors"
                outlined
                :append-icon="showRePassword ? 'mdi-eye' : 'mdi-eye-off'"
                :type="showRePassword ? 'text' : 'password'"
                @click:append="showRePassword = !showRePassword"
              ></v-text-field>
            </ValidationProvider>
          </v-col>
        </v-row>

        <v-row class="rows">
          <v-col cols="12" md="8">
            <ValidationProvider #default="{ errors }" :name="translator('email')" rules="required|email">
              <v-text-field
                v-model="model.userModel.email"
                :label="translator('email')"
                :error-messages="errors"
                outlined
              ></v-text-field>
            </ValidationProvider>
          </v-col>

          <v-col cols="12" md="4">
            <ValidationProvider
              #default="{ errors }"
              :name="translator('phone')"
              rules="required|min:10|max:11|numeric"
            >
              <v-text-field
                v-model="model.userModel.phoneNumber"
                :label="translator('phone')"
                :error-messages="errors"
                outlined
                counter="11"
              ></v-text-field>
            </ValidationProvider>
          </v-col>
        </v-row>

        <v-row class="rows">
          <v-col cols="12" md="6">
            <ValidationProvider #default="{ errors }" :name="translator('roles')" rules="required">
              <v-select
                multiple
                v-model="model.roleIdList"
                :error-messages="errors"
                :items="roles"
                item-text="value"
                item-value="id"
                :placeholder="translator('selectRole')"
                outlined
                small-chips
                required
                style="max-width: 400px;"
              ></v-select>
            </ValidationProvider>
          </v-col>

          <v-col cols="12" md="6">
            <ValidationProvider #default="{ errors }" :name="translator('priorities')" rules="required">
              <v-select
                v-model="model.userModel.priorityId"
                :items="priorities"
                item-text="name"
                item-value="id"
                :placeholder="translator('selectPriority')"
                :error-messages="errors"
                outlined
              ></v-select>
            </ValidationProvider>
          </v-col>
        </v-row>

        <v-row class="rows">
          <v-col cols="12" md="8">
            <ValidationProvider #default="{ errors }" :name="translator('companies')" rules="required">
              <v-select
                v-model="model.userModel.companyId"
                :items="companies"
                item-text="name"
                item-value="id"
                :placeholder="translator('selectCompany')"
                :error-messages="errors"
                outlined
              ></v-select>
            </ValidationProvider>
          </v-col>
        </v-row>

        <v-switch v-model="model.userModel.isActive" :label="translator('isActive')"></v-switch>

        <hr class="mb-2" />
        <v-btn @click.prevent="userCreate" color="primary" dark class="v-btn--block v-size--large mb-2">
          {{ translator("create") }}
        </v-btn>
      </v-form>
    </v-card>

    <v-dialog v-model="dialogEnable" max-width="400" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog" />
    </v-dialog>
  </ValidationObserver>
</template>

<script>
import * as rules from "vee-validate/dist/rules";
import { extend, ValidationObserver, ValidationProvider, setInteractionMode } from "vee-validate";
import MessageBox from "@/helpers/Components/MessageBox";
import CompanyService from "@/services/CompanyService";
import PriorityService from "@/services/PriorityService";
import RoleService from "@/services/RoleService";
import UserService from "@/services/UserService";
import { ReturnConstraints } from "@/helpers/Constraints";

setInteractionMode("eager");

extend("max", { ...rules.max, message: "{_field_} may not be greater than {length} characters" });
extend("min", { ...rules.min, message: "{_field_} may not be less than {length} characters" });
extend("numeric", { ...rules.numeric, message: "{_field_} can be numeric" });
extend("required", { ...rules.required, message: "{_field_} can not be empty" });

export default {
  data() {
    return {
      model: {
        userModel: {},
        roleIdList: [],
      },
      companies: [],
      priorities: [],
      roles: [],
      showPassword: false,
      showRePassword: false,
      signalModel: {
        changes: false,
        returnValues: null,
      },

      dialogEnable: false,
      dialogKey: 0,
      selectedComponent: "",
      componentProps: null,
    };
  },
  components: {
    ValidationObserver,
    ValidationProvider,
  },
  methods: {
    async initialize() {
      this.model.userModel.isActive = false;

      let companyResponse = await new CompanyService().getListAll();
      let priorityResponse = await new PriorityService().getListAll();
      let roleResponse = await new RoleService().getListAll();

      if ((companyResponse || roleResponse || priorityResponse) === ReturnConstraints.REFRESH) await this.initialize();

      this.companies = companyResponse.list;
      this.priorities = priorityResponse.list;
      this.roles = roleResponse.list;
    },
    async userCreate() {
      let result = await this.$refs.observer.validate();
      if (result) {
        let formData = new FormData();
        for (let [key, value] of Object.entries(this.model.userModel)) {
          formData.append(`${key}`, `${value}`);
        }

        formData.append("profilePhoto", this.model.userModel.profilePhoto);

        this.model.roleIdList.forEach((roleId) => {
          formData.append("roleIdList", roleId);
        });
        let response = await new UserService().create(formData);
        if (response.success) this.close("closed", true);
        else {
          this.selectedComponent = MessageBox;
          this.componentProps = {
            messageTitleProp: this.translator("failed"),
            messageTextProp: response.message,
          };
          this.dialogEnable = true;
          this.dialogKey += 1;
        }
      }
    },
    uploadImage(image) {
      this.model.userModel.profilePhoto = image;
      let reader = new FileReader();
      reader.onload = function (e) {
        document.getElementById("vimg").setAttribute("src", e.target.result.toString());
      };
      reader.readAsDataURL(image);
    },
    closeDialog() {
      this.dialogEnable = false;
      this.dialogKey += 1;
    },
    close(changes) {
      this.signalModel.changes = changes;
      this.$emit("closed", this.signalModel);
    },
  },
  created() {
    this.initialize();
  },
};
</script>

<style scoped>
.rows {
  margin-bottom: -6%;
}

.rows1 {
  margin-bottom: -3%;
}

.profileImg {
  background-size: contain;
  display: inline-block;
  height: 65px;
  margin-left: 5%;
  max-height: 65px;
}
</style>
