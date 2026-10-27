const password = document.getElementById("password");
const strength = document.getElementById("passwordStrength");

password.addEventListener("input", function () {
    const length = password.value.length;

    if (length < 8) {
        strength.textContent = "Password styrke: Svagt";
    }
    else if (length < 12) {
        strength.textContent = "Password styrke: Middel";
    }
    else {
        strength.textContent = "Password styrke: Stærkt";
    }
});