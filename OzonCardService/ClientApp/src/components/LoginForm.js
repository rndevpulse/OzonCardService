"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var React = require("react");
var react_1 = require("react");
var index_1 = require("../index");
var mobx_react_lite_1 = require("mobx-react-lite");
require("../css/LoginForm.css");
var LoginForm = function () {
    var _a = react_1.useState(''), email = _a[0], setEmail = _a[1];
    var _b = react_1.useState(''), password = _b[0], setPassword = _b[1];
    var loginstore = react_1.useContext(index_1.Context).loginstore;
    var _c = react_1.useState(false), hidden = _c[0], setHidden = _c[1];
    var classes_i = ['password material-icons'];
    if (hidden) {
        classes_i.push('red-text');
    }
    else {
        classes_i.push('black-text');
    }
    return (React.createElement("div", { className: "center form-group  col-md-6" },
        React.createElement("h5", null, "Corporate Catering Card Service"),
        React.createElement("h1", null, "Authorization"),
        React.createElement("br", null),
        React.createElement("div", { className: "autorization" },
            React.createElement("label", { htmlFor: 'email', className: "" }, "Email"),
            React.createElement("input", { id: 'email', className: "autorization__email", onChange: function (e) { return setEmail(e.target.value); }, value: email, type: 'text', placeholder: 'Email' }),
            React.createElement("label", { htmlFor: 'pass', className: "" }, "Password"),
            React.createElement("input", { id: 'pass', className: "", onChange: function (e) { return setPassword(e.target.value); }, value: password, type: !hidden ? 'password' : 'text', placeholder: 'Password' }),
            React.createElement("i", { className: classes_i.join(' '), onClick: function () { return setHidden(!hidden); } }, "remove_red_eye")),
        React.createElement("button", { className: "btn-primary button_login", onClick: function () { return loginstore.login(email, password); } }, "Login")));
};
exports.default = mobx_react_lite_1.observer(LoginForm);
//# sourceMappingURL=LoginForm.js.map