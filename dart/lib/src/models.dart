/// Response models for the Fortune Teller API.

/// API Response wrapper.
class FortunetellerResponse {
  final String status;
  final dynamic error;
  final FortunetellerData? data;

  /// Set only when the current plan locks one or more fields; null otherwise.
  final PremiumInfo? premium;

  FortunetellerResponse({
    required this.status,
    this.error,
    this.data,
    this.premium,
  });

  factory FortunetellerResponse.fromJson(Map<String, dynamic> json) => FortunetellerResponse(
    status: json['status'] as String? ?? '',
    error: json['error'],
    data: json['data'] != null ? FortunetellerData.fromJson(json['data']) : null,
    premium: json['premium'] != null ? PremiumInfo.fromJson(json['premium']) : null,
  );

  Map<String, dynamic> toJson() => {
    'status': status,
    if (error != null) 'error': error,
    if (data != null) 'data': data,
    if (premium != null) 'premium': premium!.toJson(),
  };
}

/// Describes fields the current plan does not unlock. Locked fields arrive as null
/// in [data], and [lockedFields] names them (nested ones use dot paths).
class PremiumInfo {
  final String? message;
  final String? upgradeUrl;
  final List<String>? lockedFields;

  PremiumInfo({this.message, this.upgradeUrl, this.lockedFields});

  factory PremiumInfo.fromJson(Map<String, dynamic> json) => PremiumInfo(
    message: json['message'] as String?,
    upgradeUrl: json['upgrade_url'] as String?,
    lockedFields: (json['locked_fields'] as List?)?.map((e) => e.toString()).toList(),
  );

  Map<String, dynamic> toJson() => {
    if (message != null) 'message': message,
    if (upgradeUrl != null) 'upgrade_url': upgradeUrl,
    if (lockedFields != null) 'locked_fields': lockedFields,
  };
}

/// Response data for the Fortune Teller API.

class FortunetellerData {
  String? fortune;
  String? insight;
  String? timeframe;
  String? category;
  dynamic question;
  List<num>? luckyNumbers;
  String? luckyElement;
  String? luckyColor;
  String? luckyDay;
  String? timestamp;

  FortunetellerData({
    this.fortune,
    this.insight,
    this.timeframe,
    this.category,
    this.question,
    this.luckyNumbers,
    this.luckyElement,
    this.luckyColor,
    this.luckyDay,
    this.timestamp,
  });

  factory FortunetellerData.fromJson(Map<String, dynamic> json) => FortunetellerData(
      fortune: json['fortune'],
      insight: json['insight'],
      timeframe: json['timeframe'],
      category: json['category'],
      question: json['question'],
      luckyNumbers: (json['luckyNumbers'] as List?)?.cast<num>(),
      luckyElement: json['luckyElement'],
      luckyColor: json['luckyColor'],
      luckyDay: json['luckyDay'],
      timestamp: json['timestamp'],
    );
}

class FortunetellerRequest {
  String? category;
  String? question;

  FortunetellerRequest({
    this.category,
    this.question,
  });

  Map<String, dynamic> toJson() => {
      if (category != null) 'category': category,
      if (question != null) 'question': question,
    };
}
