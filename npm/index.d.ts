declare module '@apiverve/fortuneteller' {
  export interface fortunetellerOptions {
    api_key: string;
    secure?: boolean;
  }

  /**
   * Describes fields the current plan does not unlock. Locked fields arrive as null
   * in `data`; `locked_fields` names them, using dot paths for nested fields.
   * Absent when the plan unlocks everything.
   */
  export interface PremiumInfo {
    message: string;
    upgrade_url: string;
    locked_fields: string[];
  }

  export interface fortunetellerResponse {
    status: string;
    error: string | null;
    data: FortuneTellerData;
    code?: number;
    premium?: PremiumInfo;
  }


  interface FortuneTellerData {
      fortune:      null | string;
      insight:      null | string;
      timeframe:    null | string;
      category:     null | string;
      question:     null;
      luckyNumbers: (number | null)[];
      luckyElement: null | string;
      luckyColor:   null | string;
      luckyDay:     null | string;
      timestamp:    Date | null;
  }

  export default class fortunetellerWrapper {
    constructor(options: fortunetellerOptions);

    execute(callback: (error: any, data: fortunetellerResponse | null) => void): Promise<fortunetellerResponse>;
    execute(query: Record<string, any>, callback: (error: any, data: fortunetellerResponse | null) => void): Promise<fortunetellerResponse>;
    execute(query?: Record<string, any>): Promise<fortunetellerResponse>;
  }
}
