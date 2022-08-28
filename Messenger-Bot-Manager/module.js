const Replier = {
    /**
     * @param {string} msg 
     */
    'reply': function(msg) {

    },
    /**
     * 
     * @param {string} [room]
     * @param {string} msg 
     * @param {boolean} [hideToast=false]
     * @returns {boolean}
     */
    'reply': function(room, msg, hideToast) {

    },
    /**
     * 
     * @param {string} msg 
     * @param {number} delay 
     */
    'replyDelayed': function(msg, delay) {

    },
    /**
     * 
     * @param {string} [room]
     * @param {string} msg 
     * @param {number} delay 
     * @param {boolean} [hideToast=false]
     * @returns {boolean}
     */
    'replyDelayed': function(room, msg, delay, hideToast) {
        
    }
}

const ImageDB = {
    /**
     * @returns {string}
     */
    'getProfileBase64': function() {

    },
    /**
     * 
     * @returns {string}
     */
    'getProfileImage': function() {
        return this.getProfileBase64();
    },
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'getProfileBitmap': function() {
        return null;
    },
    /**
     * @returns {string}
     */
    'getImageBase64': function() {

    },
    /**
     * @returns {string}
     */
    'getImage': function() {
        return this.getImageBase64();
    },
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
     'getImageBitmap': function() {
        return null;
    },
    /**
     * @returns {string}
     */
    'getImageHash': function() {

    },
    /**
     * @returns {string}
     */
    'getImageMD5': function() {

    },
    /**
     * @returns {string}
     */
    'getImageSHA': function() {

    }
}

const Api = {
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'getContext': () => null,
    /**
     * 
     * @param {string} scriptName
     * @returns {boolean}
     */
    'reload': (scriptName) => null,
    /**
     * 
     * @param {string} scriptName
     * @returns {boolean}
     */
    'commplie': (scriptName) => this.relod(scriptName),
    /**
     * 
     * @returns {numbr}
     */
    'prepare': (scriptName) => null,
    /**
     * 
     * @param {string} scriptName
     * @returns {boolean}
     */
    'unload': (scriptName) => null,
    /**
     * 
     * @param {string} scriptName
     * @returns {boolean}
     */
    'off': (scriptName) => null,
    /**
     * 
     * @param {string} scriptName
     * @returns {boolean}
     */
    'on': (scriptName) => null,
    /**
     * 
     * @returns {boolean}
     */
    'isOn': (scriptName) => null,
    /**
     * 
     * @param {string} scriptName
     * @returns {boolean}
     */
    'isCompiled': (scriptName) => null,
    /**
     * 
     * @param {string} scriptName
     * @returns {boolean}
     */
    'isCompiling': (scriptName) => null,
    /**
     * 
     * @returns {Array}
     */
    'getScriptNames': () => null,
    /**
     * 
     * @returns {boolean}
     */
    'replyRoom': (room, msg, hideToast) => {
        return replier.reply(room, msg, hideToast)
    },
    /**
     * 
     * @param {string} room
     * @returns {boolean}
     */
    'canReply': (room) => null,
    /**
     * 
     * @param {string} content
     * @param {int} length - 0: 짧은 시간 출력, 1: 긴시간 출력
     * @returns {undefined}
     */
    'showToast': (content, length) => null,
    /**
     * @param {string} title
     * @param {string} content
     * @param {number} [id]
     * @returns {boolean}
     */
    'makeNoti': (title, content, id) => null,
    /**
     * @param {string} sourceLanguage
     * @param {string} targetLanguage
     * @param {string} content
     * @param {boolean} errorToString
     * 
     * @returns {string}
     */
    'papagoTranslate': (sourceLanguage, targetLanguage, content, errorToString) => null,
    /**
     * 더미 함수입니다.
     * @returns {undefined}
     */
    'gc': () => null,
    /**
     * 더미 함수입니다.
     * @returns {undefined}
     */
    'UIThread': () => null,
    /**
     * 더미 함수입니다.
     * 
     * @returns {number}
     */
    'getActiveThreadsCount': () => null,
    /**
     * 더미 함수입니다.
     * 
     * @returns {undefined}
     */
    'interruptThreads': () => null,
    /**
     * 더미 함수입니다.
     * 
     * @returns {boolean}
     */
    'isTerminated': () => null,
    /**
     * 더미 함수입니다.
     * 
     * @returns {boolean}
     */
    'markAsRead': () => null,
}

const Utils = {
    /**
     * 
     * @param {string} url 
     * @returns {string}
     */
    'getWebText': (url) => null,
    /**
     * 
     * @param {string} url 
     * @returns {parse}
     */
    'parse': (url) => null,
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'getAndroidVersionCode': () => null,
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'getAndroidVersionName': () => null,
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'getPhoneBrand': () => null,
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'getPhoneModel': () => null,
}

const FileStream = {
    /**
     * 
     * @param {string} path 
     * @returns {string}
     */
    'read': (path) => null,
    /**
     * 
     * @param {string} path 
     * @param {string} data 
     * @returns {string}
     */
    'write': (path, data) => null,
    /**
     * 
     * @param {string} path 
     * @param {string} data 
     * @returns {string}
     */
    'append': (path, data) => null,
    /**
     * 
     * @param {string} path 
     * @returns {boolean}
     */
    'remove': (path) => null,
}

const DataBase = {
    /**
     * 
     * @param {string} filename 
     * @returns {string}
     */
    'getDataBase': (filename) => null,
    /**
     * 
     * @param {string} filename 
     * @param {string} data 
     * @returns {string}
     */
    'setDataBase': (filename, data) => null,
    /**
     * 
     * @param {string} filenaame 
     * @param {string} data 
     * @returns {string}
     */
    'appendDataBase': (filenaame, data) => null,
    /**
     * 
     * @param {string} filenaame 
     * @returns {boolean}
     */
    'removeDataBase': (filenaame) => null,
}

const Log = {
    /**
     * 
     * @param {string} data 
     * @param {string} showToast 
     * @returns {undefined}
     */
    'd': (data, showToast) => null,
    /**
     * 
     * @param {string} data 
     * @param {string} showToast 
     * @returns {undefined}
     */
    'debug': (data, showToast) => null,
    /**
     * 
     * @param {string} data 
     * @param {string} showToast 
     * @returns {undefined}
     */
    'e': (data, showToast) => null,
    /**
     * 
     * @param {string} data 
     * @param {string} showToast 
     * @returns {undefined}
     */
    'error': (data, showToast) => null,
    /**
     * 
     * @param {string} data 
     * @param {string} showToast 
     * @returns {undefined}
     */
    'i': (data, showToast) => null,
    /**
     * 
     * @param {string} data 
     * @param {string} showToast 
     * @returns {undefined}
     */
    'info': (data, showToast) => null,
    /**
     * 
     * @returns {undefined}
     */
    'clear': (data, showToast) => null,
}

const Device = {
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'getBuild': () => null,
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'getAndroidVersionCode': () => null,
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'getgetAndroidVersionName': () => null,
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'getPhoneBrand': () => null,
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'getPhoneModel': () => null,
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'isCharging': () => null,
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'getPlgType': () => null,
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'getBatteryLevel': () => null,
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'getBatteryHealth': () => null,
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'getBatteryTemperature': () => null,
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'getBatteryVoltage': () => null,
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'getBtteryStatus': () => null,
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'getBatteryIntent': () => null,
}

const Bridge = {
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'getScopeOf': () => null,
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'isAllowed': () => null,
}

const AppData = {
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'putBoolean': () => null,
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'getBoolean': () => null,
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'putString': () => null,
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'getString': () => null,
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'putInt': () => null,
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'getInt': () => null,
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'remove': () => null,
    /**
     * 더미 함수입니다.
     * @returns {null}
     */
    'clear': () => null,
}