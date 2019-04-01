var app = angular.module("myApp", []);

app.controller("myController", ["$scope", function ($scope) {
    $scope.SubmitLogin = function (userName, password) {
        $scope.loggedIn = userName + ", " + password;
    }
}]);