﻿using System;
using AgentMulder.ReSharper.Plugin.Components;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.UsageChecking;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AgentMulder.ReSharper.Plugin.Daemon
{
    [DaemonStage(StagesBefore = new[] { typeof(LanguageSpecificDaemonStage) })]
    public class ContainerRegistrationAnalysisStage : CSharpDaemonStageBase
    {
        private readonly SolutionAnalyzer solutionAnalyzer;

        public ContainerRegistrationAnalysisStage(SolutionAnalyzer solutionAnalyzer)
        {
            this.solutionAnalyzer = solutionAnalyzer;
        }

#if SDK70
        protected override IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind, ICSharpFile file)
#else
        public override IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, DaemonProcessKind processKind)
#endif
        
        {
            if (!IsSupported(process.SourceFile))
                return null;

            var collectUsagesStageProcess = process.GetStageProcess<CollectUsagesStageProcess>();
            var typeUsageManager = new TypeUsageManager(collectUsagesStageProcess);

            return new ContainerRegistrationAnalysisStageProcess(process, settings, typeUsageManager, solutionAnalyzer);
        }
    }
}