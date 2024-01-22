using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CaseMix.Entities.Enums
{
    public enum ExemptedBodyStructures
    {
        //General Surgery
        [Description("Laproscopic sigmoid colectomy")]
        LaproscopicSigmoidColectomy = 566463,
        [Description("Open sigmoid colectomy")]
        OpenSigmoidColectomy = 019274,
        [Description("Laproscopic appendectomy")]
        LaproscopicAppendectomy = 458500,
        [Description("Open Appendectomy")]
        OpenAppendectomy = 280491,
        [Description("Cholecystectomy")]
        Cholecystectomy = 583230,
        [Description("Appendicostomy")]
        Appendicostomy = 500107,
        [Description("Emergency Laparotomy")]
        EmergencyLaparotomy = 130804,        
        
        [Description("Right Colon Structure")]
        RightColonStructure = 51342009,
        [Description("Sigmoid Coldon Structure")]
        SigmoidColdonStructure = 60184004,

        [Description("Gastric Band")]
        GastricBand = 470413006,
        [Description("Roux-en-Y gastric bypass")]
        RouxenYGastricBypass = 173747005,
        [Description("OAGB / MGB")]
        OAGBMGB = 307297004,
        [Description("Benign Prostatic Hyperplasia (BPH)")]
        BenignProstaticHyperplasia = 266569009,
        [Description("Transurethral Resection of Prostate (TURP)")]
        TransurethralResectionProstate = 90199006,

        //Orthpedic
        [Description("Total Endoprosthesis Placement - 3d Placement")]
        TotalEndoprosthesisPlacementPlacement = 19063003,
        [Description("Total Endoprosthesis Placement - Conventional")]
        TotalEndoprosthesisPlacementConventional = 19063004,

        //[Description("Open Reduction and Internal Fixation")]
        //OpenReductionAndInternalFixation = 133861000,

        //[Description("Total Endoprostheses Placement")]
        //TotalEndoprosthesesPlacement = 47458005,

        [Description("Total Endoprosthesis Placement")]
        TotalEndoprosthesisPlacement = 47458006,

        //[Description("Bicondylar knee prosthesis")]
        //BicondylarKneeProsthesis = 109228008,

        //[Description("Correctional osteotomy of the Tibia")]
        //CorrectionalOsteotomyTibia = 31757006,
        //[Description("Cervicocapital prosthesis")]
        //CervicocapitalProsthesis = 278728008,
        //[Description("Removal of internal fixators")]
        //RemovalInternalFixators = 72010008,
        //[Description("Intermedullary nailing")]
        //IntermedullaryNailing = 734271006,
        //[Description("Wound revision")]
        //WoundRevision = 118635009,
        //[Description("ACL reconstruction")]
        //ACLReconstruction = 55244002,
        //[Description("Resection and reconstruction of the clavicle")]
        //ResectionReconstructionClavicle = 16210001,
        //[Description("Achilles tendon reconstruction")]
        //AchillesTendonReconstruction = 178156005,
        //[Description("External fixators")]
        //ExternalFixators = 239613002,
        //[Description("Arthroscopy")]
        //Arthroscopy = 13714004,
        //[Description("Carpal tunnel release")]
        //CarpalTunnelRelease = 47534009,
        //[Description("Soft tissue Exploration of the hand")]
        //SoftTssueExplorationHand = 10000001,
        //[Description("Haematoma evacuation")]
        //HaematomaEvacuation = 118441006,
        //[Description("Decompression and adhesiolysis of periferal nerves")]
        //DecompressionAdhesiolysisPeriferalNerves = 38663004,
        //[Description("Exostosis Removal")]
        //ExostosisRemoval = 392033004,

        //Cardiology
        [Description("Percutaneous Coronary Intervention")]
        PercutaneousCoronaryIntervention = 415070008,
        [Description("Electrophysiology")]
        Electrophysiology = 252425004
    }
}
